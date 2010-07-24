using System.Runtime.Serialization;

namespace CloudServiceBus.Entities.Enums
{
    [DataContract]
    public enum ServiceResponseState
    {
        [EnumMember]
        Completed, 

        [EnumMember]
        Errored
    }
}
