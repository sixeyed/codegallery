using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls.WebParts;
using WcfPersonalizationSample.ServiceClient.PersonalizationService;

namespace WcfPersonalizationSample.ServiceClient
{
    /// <summary>
    /// Personalization provider which access the ASP.NET SQL Personalization
    /// store across a WCF service
    /// </summary>
    /// <remarks>
    /// Requires configuration for the IPersonalizationService providing the data
    /// </remarks>
    public class WcfPersonalizationProvider : PersonalizationProvider
    {
        #region Private properties
        
        private PersonalizationServiceClient _client;
        private PersonalizationServiceClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new PersonalizationServiceClient();
                }
                return _client;
            }
        }
        #endregion

        #region PersonalizationProvider implementation

        /// <summary>
        /// Gets/sets the application name for the store
        /// </summary>
        public override string ApplicationName { get; set; }

        /// <summary>
        /// Finds state matching given parameters
        /// </summary>
        /// <param name="scope">Scope to search</param>
        /// <param name="query">Query to action</param>
        /// <param name="pageIndex">Index of page to retrieve</param>
        /// <param name="pageSize">Size of page to retrieve</param>
        /// <param name="totalRecords">Count of records returned</param>
        /// <returns>Matching state</returns>
        public override PersonalizationStateInfoCollection FindState(PersonalizationScope scope, PersonalizationStateQuery query, int pageIndex, int pageSize, out int totalRecords)
        {
            FindStateResult result = Client.FindState(scope, query, pageIndex, pageSize);
            totalRecords = result.TotalRecords;
            return result.StateInfoCollection;
        }

        /// <summary>
        /// Gets the count of items in state
        /// </summary>
        /// <param name="scope">Scope to count</param>
        /// <param name="query">Query to action</param>
        /// <returns>Count of state</returns>
        public override int GetCountOfState(PersonalizationScope scope, PersonalizationStateQuery query)
        {
            return Client.GetCountOfState(scope, query);
        }

        /// <summary>
        /// Loads shared and user personalization data blobs
        /// </summary>
        /// <param name="webPartManager">Current web part manager</param>
        /// <param name="path">Path to data</param>
        /// <param name="userName">User name</param>
        /// <param name="sharedDataBlob">Blob of shared data</param>
        /// <param name="userDataBlob">Blob of user data</param>
        protected override void LoadPersonalizationBlobs(WebPartManager webPartManager, string path, string userName, ref byte[] sharedDataBlob, ref byte[] userDataBlob)
        {
            sharedDataBlob = Client.GetSharedPersonalizationBlob(path, userName);
            userDataBlob = Client.GetUserPersonalizationBlob(path, userName);
        }

        /// <summary>
        /// Resets personalization data
        /// </summary>
        /// <param name="webPartManager">Current web part manager</param>
        /// <param name="path">Path to data</param>
        /// <param name="userName">User name</param>
        protected override void ResetPersonalizationBlob(WebPartManager webPartManager, string path, string userName)
        {
            Client.ResetPersonalizationBlob(path, userName);
        }

        /// <summary>
        /// Resets personalization state
        /// </summary>
        /// <param name="scope">Scope to reset</param>
        /// <param name="paths">Paths to data</param>
        /// <param name="usernames">Usernames to reset</param>
        /// <returns>Count of reset users</returns>
        public override int ResetState(PersonalizationScope scope, string[] paths, string[] usernames)
        {
            return Client.ResetState(scope, paths, usernames);
        }

        /// <summary>
        /// Resets user personalization state
        /// </summary>
        /// <param name="path">Path to data</param>
        /// <param name="userInactiveSinceDate">Date to clear inactive users</param>
        /// <returns>Count of reset users</returns>
        public override int ResetUserState(string path, DateTime userInactiveSinceDate)
        {
            return Client.ResetUserState(path, userInactiveSinceDate);
        }

        /// <summary>
        /// Saves personalization blob
        /// </summary>
        /// <param name="webPartManager"></param>
        /// <param name="path">Path to data</param>
        /// <param name="userName">User name</param>
        /// <param name="dataBlob">Data blob</param>
        protected override void SavePersonalizationBlob(WebPartManager webPartManager, string path, string userName, byte[] dataBlob)
        {
            Client.SavePersonalizationBlob(path, userName, dataBlob);
        }

        #endregion
    }
}
