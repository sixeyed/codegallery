using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;

namespace Sixeyed.OptimisticLockingSample.ServiceModel.Helpers
{
    /// <summary>
    /// Helper class for extracting values from IExtensibleDataObject 
    /// </summary>
    /// <remarks>
    /// Relies on non-public members of ExtensionDataObject
    /// </remarks>
    public static class ExtensionDataHelper
    {
        /// <summary>
        /// Returns the value of the named data member from the ExtensionData property of the
        /// IExtensibleDataObject. Returns null if no data member found
        /// </summary>
        /// <param name="extensibleObject">Object to search</param>
        /// <param name="dataMemberName">Name of data member to find</param>
        /// <returns>Data member value or null</returns>
        public static object GetExtensionDataMemberValue(IExtensibleDataObject extensibleObject, string dataMemberName)
        {
            object innerValue = null;
            PropertyInfo membersProperty = typeof(ExtensionDataObject).GetProperty("Members", BindingFlags.NonPublic | BindingFlags.Instance);
            IList members = (IList)membersProperty.GetValue(extensibleObject.ExtensionData, null);
            if (members != null)
            {
                foreach (object member in members)
                {
                    PropertyInfo nameProperty = member.GetType().GetProperty("Name", BindingFlags.NonPublic | BindingFlags.Instance);
                    string name = (string)nameProperty.GetValue(member, null);
                    if (name == dataMemberName)
                    {
                        PropertyInfo valueProperty = member.GetType().GetProperty("Value", BindingFlags.NonPublic | BindingFlags.Instance);
                        object value = valueProperty.GetValue(member, null);
                        PropertyInfo innerValueProperty = value.GetType().GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);
                        innerValue = innerValueProperty.GetValue(value, null);
                        break;
                    }
                }
            }
            return innerValue;
        }
    }
}
