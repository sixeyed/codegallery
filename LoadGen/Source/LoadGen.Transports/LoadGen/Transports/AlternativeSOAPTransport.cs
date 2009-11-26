
#region Directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using LoadGen;
using LoadGenHelper;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Security.Permissions;
using System.Xml;

#endregion

namespace LoadGen.Transports
{
    /// <summary>
    /// Alternative SOAP transport which reads a full SOAP envelope - header + body from the input file
    /// </summary>
    /// <remarks>
    /// Functionally similar to LoadGen.SOAPTransport but without the configured parameters for SOAP 
    /// envelope prefix and postfix - this transport uses the whole input message as the SOAP message
    /// </remarks>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust"), PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
    public class AlternativeSOAPTransport : ITransport, IConfigurable, IDisposable
    {
        #region Private instance fields

        private bool _logLatency;
        private bool _saveResponseMessage;
        private bool _useIntegratedAuthentication;
        private string[] _destinationLocation;
        private DateTime _startDateTime;
        private Message _messageToBeSent = new Message();
        private ASCIIEncoding _asciiEncoding = new ASCIIEncoding();
        private UTF8Encoding _utf8Encoding = new UTF8Encoding();
        private EncodingConverter _converter = new EncodingConverter();
        private Stream _httpStream;
        private HttpWebRequest _httpRequest;
        private HttpWebResponse _httpResponse;
        private static ReaderWriterLock _lock = new ReaderWriterLock();
        private string _destinationEncoding;
        private string _latencyFileName;
        private string _responseMessage;
        private string _saveResponseMessagePath;
        private string _soapHeader;
        private string _url;
        private FileStream _latencyFileStream;
        private StreamWriter _latencyFileWriter;
        private byte[] _buffer = new byte[0x1000];
        private PerformanceCounter _rateSentCounter;
        private PerformanceCounter _totalSentCounter;

        #endregion

        #region Public instance properties

        /// <summary>
        /// Gets/sets the SOAP destination location
        /// </summary>
        public string[] DstLocation
        {
            get
            {
                return this._destinationLocation;
            }
            set
            {
                this._destinationLocation = value;
                if (this._destinationLocation.Length < 1)
                {
                    throw new ConfigException("<URL/> Missing!" + Environment.NewLine);
                }
                this._url = value[0];
                if (this._url.Length == 0)
                {
                    throw new ConfigException("<URL/> Empty!" + Environment.NewLine);
                }
                if (this._destinationLocation.Length < 2)
                {
                    throw new ConfigException("<SOAPHeader/> Missing!" + Environment.NewLine);
                }
                this._soapHeader = value[1];
                if (this._soapHeader.Length == 0)
                {
                    throw new ConfigException("<SOAPHeader/> Empty!" + Environment.NewLine);
                }
                this._useIntegratedAuthentication = false;
                if (this._destinationLocation.Length > 3)
                {
                    this._useIntegratedAuthentication = Convert.ToBoolean(value[3]);
                }
                this._logLatency = false;
                if (this._destinationLocation.Length > 4)
                {
                    this._latencyFileName = value[4];
                    if (this._latencyFileName.Length != 0)
                    {
                        this._logLatency = true;
                    }
                }
                this._saveResponseMessage = false;
                if (this._destinationLocation.Length > 5)
                {
                    this._saveResponseMessagePath = value[5];
                    if (this._saveResponseMessagePath.Length != 0)
                    {
                        this._saveResponseMessage = true;
                    }
                }
                if (this._destinationLocation.Length > 6)
                {
                    this._destinationEncoding = value[6];
                }
                else
                {
                    this._destinationEncoding = "";
                }
            }
        }

        /// <summary>
        /// Gets/sets the message to be sent to LoadGen
        /// </summary>
        public Message MessageToBeSent
        {
            get { return this._messageToBeSent; }
            set { this._messageToBeSent.Clone(value); }
        }

        /// <summary>
        /// Gets/sets the configuration for the adapter
        /// </summary>
        public string ConfigParameters
        {
            get
            {
                return "<Parameters><URL/><SOAPHeader/><IsUseIntegratedAuth/><LatencyFileName/><ResponseMsgPath/></Parameters>";
            }
            set
            {
                StringReader txtReader = new StringReader(value);
                XmlDocument document = new XmlDocument();
                document.Load(txtReader);
                if (document.SelectSingleNode("/Parameters/URL") == null)
                {
                    throw new ConfigException("<URL/> Missing!" + Environment.NewLine);
                }
                this._url = document.SelectSingleNode("/Parameters/URL").InnerText;
                if (this._url.Length == 0)
                {
                    throw new ConfigException("<URL/> Empty!" + Environment.NewLine);
                }
                if (document.SelectSingleNode("/Parameters/SOAPHeader") == null)
                {
                    throw new ConfigException("<SOAPHeader/> Missing!" + Environment.NewLine);
                }
                this._soapHeader = document.SelectSingleNode("/Parameters/SOAPHeader").InnerText;
                if (this._soapHeader.Length == 0)
                {
                    throw new ConfigException("<SOAPHeader/> Empty!" + Environment.NewLine);
                }
                if (document.SelectSingleNode("/Parameters/IsUseIntegratedAuth") != null)
                {
                    this._useIntegratedAuthentication = Convert.ToBoolean(document.SelectSingleNode("/Parameters/IsUseIntegratedAuth").InnerText);
                }
                if (document.SelectSingleNode("/Parameters/LatencyFileName") != null)
                {
                    this._latencyFileName = document.SelectSingleNode("/Parameters/LatencyFileName").InnerText;
                    this._logLatency = this._latencyFileName.Length != 0;
                }
                if (document.SelectSingleNode("/Parameters/ResponseMsgPath") != null)
                {
                    this._saveResponseMessagePath = document.SelectSingleNode("/Parameters/ResponseMsgPath").InnerText;
                    this._saveResponseMessage = this._saveResponseMessagePath.Length != 0;
                }
                if (document.SelectSingleNode("/Parameters/DstEncoding") != null)
                {
                    this._destinationEncoding = document.SelectSingleNode("/Parameters/DstEncoding").InnerText;
                }
                txtReader.Dispose();
            }
        }

        #endregion

        #region Public instance methods

        /// <summary>
        /// Initialises the transport
        /// </summary>
        /// <param name="sInitialize">UNUSED</param>
        public void Initialize(string sInitialize)
        {
            if (this._logLatency && (this._latencyFileName.Length != 0))
            {
                this._latencyFileStream = new FileStream(this._latencyFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                this._latencyFileWriter = new StreamWriter(this._latencyFileStream);
            }
            string instanceName = LoadGenPerfCounters.CreateInstanceName(this._url);
            this._totalSentCounter = LoadGenPerfCounters.CreatePerfCounter(PerfCategoryID.LoadGen_SOAPTransport, PerfCounterID.Messages_Sent, instanceName, LoadGenHelper.TransportType.Transport);
            this._rateSentCounter = LoadGenPerfCounters.CreatePerfCounter(PerfCategoryID.LoadGen_SOAPTransport, PerfCounterID.Messages_Sent_Per_Sec, instanceName, LoadGenHelper.TransportType.Transport);
        }

        /// <summary>
        /// Sends a large message using the given input file
        /// </summary>
        /// <param name="uniqueDestinationFileName">Input file consisting of message to send</param>
        public void SendLargeMessage(string uniqueDestinationFileName)
        {
            this._httpRequest = (HttpWebRequest)WebRequest.Create(this._url);
            this._httpRequest.ServicePoint.ConnectionLimit = 0x3e8;
            this._httpRequest.Method = "POST";
            this._httpRequest.KeepAlive = true;
            this._httpRequest.Timeout = 0x1e8480;
            this._httpRequest.ContentType = "text/xml; charset=utf-8";
            this._httpRequest.ContentLength = this.MessageToBeSent.MessageLength;
            this._httpRequest.Headers.Add(this._soapHeader);
            if (this._useIntegratedAuthentication)
            {
                this._httpRequest.Credentials = CredentialCache.DefaultCredentials;
            }
            FileStream input = new FileStream(this.MessageToBeSent.MessagePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader reader = new BinaryReader(input);
            BinaryWriter writer = new BinaryWriter(this._httpRequest.GetRequestStream());
            int count = 0;
            long num2 = 0L;
            if (this._logLatency)
            {
                this._startDateTime = DateTime.Now;
            }
            while (num2 < this.MessageToBeSent.MessageLength)
            {
                count = reader.Read(this._buffer, 0, this._buffer.Length);
                writer.Write(this._buffer, 0, count);
                num2 += count;
            }
            writer.Close();
            this._totalSentCounter.Increment();
            this._rateSentCounter.Increment();
            reader.Close();
            input.Close();
            this._httpResponse = (HttpWebResponse)this._httpRequest.GetResponse();
            if (this._httpResponse != null)
            {
                StreamReader reader2 = null;
                reader2 = new StreamReader(this._httpResponse.GetResponseStream());
                this._responseMessage = reader2.ReadToEnd();
                reader2.Close();
            }
            this._httpResponse.Close();
            if (this._logLatency)
            {
                this.LogLatencyInfo(uniqueDestinationFileName, DateTime.Now.Subtract(this._startDateTime).TotalMilliseconds);
            }
            if (this._saveResponseMessage)
            {
                this.SaveResponseMsg(uniqueDestinationFileName);
            }
        }

        /// <summary>
        /// Sends a small message using the given input file
        /// </summary>
        /// <param name="uniqueDestinationFileName">Input file consisting of message to send</param>
        public void SendSmallMessage(string uniqueDestinationFileName)
        {
            this._httpRequest = (HttpWebRequest)WebRequest.Create(this._url);
            this._httpRequest.ServicePoint.ConnectionLimit = 0x3e8;
            this._httpRequest.Method = "POST";
            this._httpRequest.KeepAlive = true;
            this._httpRequest.Timeout = 0x1e8480;
            if (string.IsNullOrEmpty(this._destinationEncoding))
            {
                this._destinationEncoding = this._converter.GetDataEncoding(this.MessageToBeSent.MessageData);
            }
            byte[] buffer = this._converter.ConvertBuffer(this.MessageToBeSent.MessageData, this._destinationEncoding);
            this._httpRequest.ContentType = "text/xml; charset=" + this._destinationEncoding;
            this._httpRequest.ContentLength = buffer.Length;
            this._httpRequest.Headers.Add(this._soapHeader);
            if (this._useIntegratedAuthentication)
            {
                this._httpRequest.Credentials = CredentialCache.DefaultCredentials;
            }
            this._httpStream = this._httpRequest.GetRequestStream();
            if (this._logLatency)
            {
                this._startDateTime = DateTime.Now;
            }
            this._httpStream.Write(buffer, 0, buffer.Length);
            this._httpStream.Close();
            this._totalSentCounter.Increment();
            this._rateSentCounter.Increment();
            this._httpResponse = (HttpWebResponse)this._httpRequest.GetResponse();
            if (this._httpResponse != null)
            {
                StreamReader reader = null;
                reader = new StreamReader(this._httpResponse.GetResponseStream());
                this._responseMessage = reader.ReadToEnd();
                reader.Close();
            }
            this._httpResponse.Close();
            if (this._logLatency)
            {
                this.LogLatencyInfo(uniqueDestinationFileName, DateTime.Now.Subtract(this._startDateTime).TotalMilliseconds);
            }
            if (this._saveResponseMessage)
            {
                this.SaveResponseMsg(uniqueDestinationFileName);
            }
        }

        /// <summary>
        /// Cleans up the adapter
        /// </summary>
        /// <param name="sCleanup">UNUSED</param>
        public void Cleanup(string sCleanup)
        {
            this.Dispose();
        }

        /// <summary>
        /// Disposes resources
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~AlternativeSOAPTransport()
        {
            this.Dispose(false);
        }

        #endregion        

        #region Protected instance methods

        /// <summary>
        /// Disposes resources
        /// </summary>
        /// <param name="disposing">Whether disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._logLatency && (this._latencyFileStream != null))
                {
                    this._latencyFileWriter.Flush();
                    this._latencyFileWriter.Close();
                    this._latencyFileWriter = null;
                    this._latencyFileStream.Close();
                    this._latencyFileStream = null;
                }
                if (this._totalSentCounter != null)
                {
                    this._totalSentCounter.Close();
                }
                if (this._rateSentCounter != null)
                {
                    this._rateSentCounter.Close();
                }
            }
        }

        #endregion

        #region Private instance methods

        private void LogLatencyInfo(string UniqueDestFileName, double dblDeltaTime)
        {
            _lock.AcquireWriterLock(-1);
            try
            {
                this._latencyFileWriter.WriteLine(UniqueDestFileName + "," + dblDeltaTime);
            }
            finally
            {
                _lock.ReleaseWriterLock();
            }
        }

        private void SaveResponseMsg(string uniqueDestinationFileName)
        {
            FileStream output = null;
            BinaryWriter writer = null;
            FileStream stream2 = null;
            BinaryWriter writer2 = null;
            try
            {
                output = new FileStream(this._saveResponseMessagePath + @"\" + uniqueDestinationFileName + "-INP.Xml", FileMode.CreateNew);
                writer = new BinaryWriter(output);
                writer.Write(this.MessageToBeSent.MessageData);
                stream2 = new FileStream(this._saveResponseMessagePath + @"\" + uniqueDestinationFileName + "-RSP.Xml", FileMode.CreateNew);
                writer2 = new BinaryWriter(stream2);
                byte[] bytes = this._utf8Encoding.GetBytes(this._responseMessage);
                writer2.Write(bytes);
            }
            catch (Exception exception)
            {
                EventLog.WriteEntry("LoadGen", "SOAPTransport - SaveResponseMsg() encountered the following exception and it will retry: " + exception.ToString(), EventLogEntryType.Error);
                throw;
            }
            finally
            {
                if (output != null)
                {
                    output.Close();
                }
                if (writer != null)
                {
                    writer.Close();
                }
                if (stream2 != null)
                {
                    stream2.Close();
                }
                if (writer2 != null)
                {
                    writer2.Close();
                }
            }
        }

        #endregion
    }
}