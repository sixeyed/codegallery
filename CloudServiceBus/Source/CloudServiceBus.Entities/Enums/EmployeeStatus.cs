using System.Runtime.Serialization;

namespace CloudServiceBus.Entities.Enums
{
    [DataContract]
    public enum EmployeeStatus
    {
        [EnumMember]
        Active, 

        [EnumMember]
        Inactive
    }
}
