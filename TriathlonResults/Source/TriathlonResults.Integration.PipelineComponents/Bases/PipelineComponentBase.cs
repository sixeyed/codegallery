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
using Microsoft.Practices.ESB;
using System.Globalization;

namespace TriathlonResults.Integration.PipelineComponents.Bases
{    
    public abstract class PipelineComponentBase :
        IBaseComponent,
        Microsoft.BizTalk.Component.Interop.IComponent,
        Microsoft.BizTalk.Component.Interop.IPersistPropertyBag,
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

        protected abstract Guid Guid { get;}

        #endregion

        #region IComponentUI members

        public IEnumerator Validate(object obj)
        {
            return null;
        }

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

        public void InitNew()
        {
        }

        public void Load(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, Int32 errlog)
        {
        }

        public void Save(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, Boolean fClearDirty, Boolean fSaveAllProperties)
        {
        }

        public void GetClassID(out Guid classID)
        {
            classID = this.Guid;
        }

        #endregion

        #region IBaseComponent members

        [Browsable(false)]
        public string Name
        {
            get
            {
                return this.ResourceManager.GetString("COMPONENTNAME", CultureInfo.InvariantCulture);
            }
        }

        [Browsable(false)]
        public string Version
        {
            get
            {
                return this.ResourceManager.GetString("COMPONENTVERSION", CultureInfo.InvariantCulture);
            }
        }

        [Browsable(false)]
        public string Description
        {
            get
            {
                return this.ResourceManager.GetString("COMPONENTDESCRIPTION", CultureInfo.InvariantCulture);
            }
        }

        #endregion

        #region IComponent members

        public abstract IBaseMessage Execute(IPipelineContext pc, IBaseMessage inmsg);

        #endregion
    }
}