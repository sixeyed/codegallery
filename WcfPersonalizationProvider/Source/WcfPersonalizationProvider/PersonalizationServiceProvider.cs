using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls.WebParts;

namespace WcfPersonalizationProvider
{
    /// <summary>
    /// Personalization provider which exposes <see cref="SqlPersonalizationProvider"/>
    /// functionality across a WCF service
    /// </summary>
    public class PersonalizationServiceProvider : SqlPersonalizationProvider
    {
        /// <summary>
        /// Gets the blob of shared personalization data
        /// </summary>
        /// <param name="path">Path to data</param>
        /// <param name="userName">User name</param>
        /// <returns>Shared data blob</returns>
        public byte[] GetSharedPersonalizationBlob(string path, string userName)
        {
            byte[] sharedDataBlob = null;
            byte[] userDataBlob = null;
            base.LoadPersonalizationBlobs(null, path, userName, ref sharedDataBlob, ref userDataBlob);
            return sharedDataBlob;
        }

        /// <summary>
        /// Gets the blob of user-specific personalization data
        /// </summary>
        /// <param name="path">Path to data</param>
        /// <param name="userName">User name</param>
        /// <returns>User data blob</returns>
        public byte[] GetUserPersonalizationBlob(string path, string userName)
        {
            byte[] sharedDataBlob = null;
            byte[] userDataBlob = null;
            base.LoadPersonalizationBlobs(null, path, userName, ref sharedDataBlob, ref userDataBlob);
            return userDataBlob;
        }

        /// <summary>
        /// Resets personalization blob
        /// </summary>
        /// <param name="path">Path to data</param>
        /// <param name="userName">User name</param>
        public void ResetPersonalizationBlob(string path, string userName)
        {
            base.ResetPersonalizationBlob(null, path, userName);
        }

        /// <summary>
        /// Saves personalization blob
        /// </summary>
        /// <param name="path">Path to data</param>
        /// <param name="userName">User name</param>
        /// <param name="dataBlob">Data blob</param>
        public void SavePersonalizationBlob(string path, string userName, byte[] dataBlob)
        {
            base.SavePersonalizationBlob(null, path, userName, dataBlob);
        }
    }
}