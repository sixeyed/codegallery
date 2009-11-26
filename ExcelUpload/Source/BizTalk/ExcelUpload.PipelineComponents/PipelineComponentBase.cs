using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Resources;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Component;
using Microsoft.BizTalk.Messaging;
using System.Xml;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Globalization;

namespace ExcelUpload.PipelineComponents
{    
    /// <summary>
    /// Base class for generic pipeline components
    /// </summary>
    public abstract class PipelineComponentBase :
        IBaseComponent,
        IPersistPropertyBag,
        IComponentUI
    {
        #region Private instance fields

        private ResourceManager _resourceManager;
        
        #endregion

        #region Private instance properties

        private ResourceManager ResourceManager
        {
            get
            {
                if (this._resourceManager == null)
                {
                    this._resourceManager = new ResourceManager(this.GetType());
                }
                return this._resourceManager;
            }
        }

        #endregion

        #region Protected abstract properties

        /// <summary>
        /// Returns the GUID of the component
        /// </summary>
        protected abstract Guid Guid { get;}

        #endregion

        #region Protected methods

        /// <summary>
        /// Reads property value from property bag
        /// </summary>
        /// <param name="propertyBag">Property bag</param>
        /// <param name="propertyName">Name of property</param>
        /// <returns>Value of the property</returns>
        protected static object ReadPropertyBag(IPropertyBag propertyBag, string propertyName)
        {
            object val = null;
            try
            {
                propertyBag.Read(propertyName, out val, 0);
            }
            catch (System.ArgumentException)
            {
                return val;
            }
            return val;
        }

        /// <summary>
        /// Writes property values into a property bag.
        /// </summary>
        /// <param name="propertyBag">Property bag.</param>
        /// <param name="propertyName">Name of property.</param>
        /// <param name="value">Value of property.</param>
        protected static void WritePropertyBag(IPropertyBag propertyBag, string propertyName, object value)
        {
            propertyBag.Write(propertyName, ref value);
        }

        #endregion

        #region IComponentUI members

        /// <summary>
        /// Validate the component configuration
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>IEnumerator</returns>
        public virtual IEnumerator Validate(object obj)
        {
            //base implementation - no validation, return success result:
            return new ArrayList(0).GetEnumerator();
        }

        /// <summary>
        /// Returns a pointer to the icon for the component
        /// </summary>
        /// <remarks>
        /// Read from the COMPONENTICON resource for the child class
        /// </remarks>
        [Browsable(false)]
        public IntPtr Icon
        {
            get
            {
                return ((System.Drawing.Bitmap)(this.ResourceManager.GetObject("COMPONENTICON", System.Globalization.CultureInfo.InvariantCulture))).GetHicon();
            }

        }

        #endregion

        #region IPersistPropertyBag members

        /// <summary>
        /// Initializes a new configured instance
        /// </summary>
        /// <remarks>
        /// Not implemented in base class
        /// </remarks>
        public virtual void InitNew() {}
        
        /// <summary>
        /// Loads configuration from property bag
        /// </summary>
        /// <remarks>
        /// Not implemented in base class
        /// </remarks>
        /// <param name="pb"></param>
        /// <param name="errlog"></param>
        public virtual void Load(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, Int32 errlog) {}

        /// <summary>
        /// Saves configuration to property bag
        /// </summary>
        /// <remarks>
        /// Not implemented in base class
        /// </remarks>
        /// <param name="pb"></param>
        /// <param name="fClearDirty"></param>
        /// <param name="fSaveAllProperties"></param>
        public virtual void Save(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, Boolean fClearDirty, Boolean fSaveAllProperties) {}

        /// <summary>
        /// Returns the class GUID of the component
        /// </summary>
        /// <param name="classID">Class GUID</param>
        public void GetClassID(out Guid classID)
        {
            classID = this.Guid;
        }

        #endregion

        #region IBaseComponent members

        /// <summary>
        /// Returns the name of the component
        /// </summary>
        /// <remarks>
        /// Read from the COMPONENTNAME resource for the child class
        /// </remarks>
        [Browsable(false)]
        public string Name
        {
            get
            {
                return this.ResourceManager.GetString("COMPONENTNAME", CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Returns the version of the component
        /// </summary>
        /// <remarks>
        /// Read as the version number of the component's assembly
        /// </remarks>
        [Browsable(false)]
        public virtual string Version
        {
            get
            {
                return this.GetType().Assembly.GetName().Version.ToString(2);
            }
        }

        /// <summary>
        /// Returns the description of the component
        /// </summary>
        /// <remarks>
        /// Read from the COMPONENTDESCRIPTION resource for the child class
        /// </remarks>
        [Browsable(false)]
        public string Description
        {
            get
            {
                return this.ResourceManager.GetString("COMPONENTDESCRIPTION", CultureInfo.InvariantCulture);
            }
        }

        #endregion
    }
}