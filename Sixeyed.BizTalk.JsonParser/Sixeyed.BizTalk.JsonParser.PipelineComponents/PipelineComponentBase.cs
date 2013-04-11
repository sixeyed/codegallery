using Microsoft.BizTalk.Component.Interop;
using System;

namespace Sixeyed.BizTalk.JsonParser.PipelineComponents
{
    public abstract class PipelineComponentBase : IBaseComponent, IComponentUI, IPersistPropertyBag
    {  
        #region IBaseComponent Members
        /// <summary>
        /// The PLC description
        /// </summary>
        public abstract string Description { get;}       
        /// <summary>
        /// Return the PLC name
        /// </summary>
        public string Name
        {
            get { return this.GetType().Name; }
        }
        /// <summary>
        /// The PLC version number
        /// </summary>
        public string Version
        {
            get { return this.GetType().Assembly.GetName().Version.ToString(); }
        }
        #endregion
 
        #region IComponentUI Members
        /// <summary>
        /// Icon
        /// </summary>
        public IntPtr Icon
        {
            get { return IntPtr.Zero; }
        }
        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="projectSystem"></param>
        /// <returns></returns>
        public System.Collections.IEnumerator Validate(object projectSystem)
        {
            return null;
        }
        #endregion
 
        #region IPersistPropertyBag Members
 
        public void GetClassID(out Guid classID)
        {
            classID = GetType().GUID;
        }

        public void InitNew() { }
 
        public abstract void Load(IPropertyBag propertyBag, int errorLog);
 
        public abstract void Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties);
 
        #endregion

        protected static object ReadPropertyBag(IPropertyBag propertyBag, string propName)
        {
            object val = null;
            try
            {
                propertyBag.Read(propName, out val, 0);
            }
            catch (System.ArgumentException)
            {
                return val;
            }
            return val;
        }

        protected static void WritePropertyBag(IPropertyBag propertyBag, string propName, object val)
        {
            propertyBag.Write(propName, ref val);
        }
    }
}

