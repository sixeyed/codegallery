using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Web.UI.WebControls.WebParts;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace WcfPersonalizationProvider
{
    [ServiceContract]
    public interface IPersonalizationService
    {
        [OperationContract]
        [ServiceKnownType(typeof(PersonalizationScope))]
        [ServiceKnownType(typeof(PersonalizationStateQuery))]
        FindStateResult FindState(PersonalizationScope scope, PersonalizationStateQuery query, int pageIndex, int pageSize);

        [OperationContract]
        [ServiceKnownType(typeof(PersonalizationScope))]
        [ServiceKnownType(typeof(PersonalizationStateQuery))]
        int GetCountOfState(PersonalizationScope scope, PersonalizationStateQuery query);

        [OperationContract]
        byte[] GetSharedPersonalizationBlob(string path, string userName);

        [OperationContract]
        byte[] GetUserPersonalizationBlob(string path, string userName);

        [OperationContract]
        void ResetPersonalizationBlob(string path, string userName);

        [OperationContract]
        [ServiceKnownType(typeof(PersonalizationScope))]
        int ResetState(PersonalizationScope scope, string[] paths, string[] usernames);

        [OperationContract]
        int ResetUserState(string path, DateTime userInactiveSinceDate);

        [OperationContract]
        void SavePersonalizationBlob(string path, string userName, byte[] dataBlob);
    }

    [DataContract]
    [KnownType(typeof(PersonalizationStateInfoCollection))]
    [KnownType(typeof(UserPersonalizationStateInfo))]
    [KnownType(typeof(SharedPersonalizationStateInfo))]
    public class FindStateResult
    {
        [DataMember]
        public PersonalizationStateInfoCollection StateInfoCollection { get; set; }

        [DataMember]
        public int TotalRecords { get; set; }
    }
}


