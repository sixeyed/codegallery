using System;
using System.Runtime.Serialization;
using CloudServiceBus.Entities.Enums;

namespace CloudServiceBus.Entities
{
    [DataContract(Namespace = TargetNamespace.CloudServiceBus)]
    public class Employee
    {
        [DataMember]
        public string RoleTitle { get; set; }

        [DataMember]
        public string OfficeCountry { get; set; }

        [DataMember]
        public string EmailAddress { get; set; }

        [DataMember]
        public EmployeeStatus Status { get; set; }

        [DataMember]
        public string DepartmentName { get; set; }

        [DataMember]
        public string EmployeeId { get; set; }

        [DataMember]
        public string OfficeLocation { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string FaxNumber { get; set; }

        [DataMember]
        public DateTime LatestHireDate { get; set; }

        [DataMember]
        public string RoleDescription { get; set; }

        [DataMember]
        public string MobileNumber { get; set; }

        [DataMember]
        public string DirectDialNumber { get; set; }

        [DataMember]
        public string ExtensionNumber { get; set; }

        [DataMember]
        public DateTime LastChangeDate { get; set; }

        public override string ToString()
        {
            return string.Format("Employee[EmployeeId:{0}, FullName:{1}]", EmployeeId, FullName);
        }
    }
}
