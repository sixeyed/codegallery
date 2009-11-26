using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Web.UI.WebControls.WebParts;
using System.Collections.ObjectModel;

namespace WcfPersonalizationProvider
{
    /// <summary>
    /// Service implementation for accessing SQL personalization store
    /// </summary>
    /// <remarks>
    /// Requires a <see cref="PersonalizationServiceProvider"/> to be configured
    /// in the host in the system.web section as the default personalization provider
    /// </remarks>
    /// <example>
    ///    <system.web>
    ///     <webParts enableExport="true">
    ///      <personalization defaultProvider="PersonalizationServiceProvider">
    ///        <providers>
    ///          <add name="PersonalizationServiceProvider"
    ///            type="WcfPersonalizationProvider.PersonalizationServiceProvider"
    ///            connectionStringName="local"
    ///            applicationName="WcfPersonalizationSample" />
    ///        </providers>
    ///      </personalization>
    ///    </webParts>
    ///    ...
    /// </example>
    public class PersonalizationService : IPersonalizationService
    {
        #region Private properties

        private PersonalizationServiceProvider _provider;
        private PersonalizationServiceProvider Provider
        {
            get
            {
                if (_provider == null)
                {
                    _provider = (PersonalizationServiceProvider)PersonalizationAdministration.Provider;
                }
                return _provider;
            }
        }

        #endregion

        #region IPersonalizationService implementation

        /// <summary>
        /// Finds state matching given parameters
        /// </summary>
        /// <param name="scope">Scope to search</param>
        /// <param name="query">Query to action</param>
        /// <param name="pageIndex">Index of page to retrieve</param>
        /// <param name="pageSize">Size of page to retrieve</param>
        /// <returns>Matching state</returns>
        public FindStateResult FindState(PersonalizationScope scope, PersonalizationStateQuery query, int pageIndex, int pageSize)
        {
            int totalRecords = 0;
            PersonalizationStateInfoCollection collection = Provider.FindState(scope, query, pageIndex, pageSize, out totalRecords);
            FindStateResult result = new FindStateResult();
            result.TotalRecords = totalRecords;
            result.StateInfoCollection = collection;
            return result;
        }

        /// <summary>
        /// Gets the count of items in state
        /// </summary>
        /// <param name="scope">Scope to count</param>
        /// <param name="query">Query to action</param>
        /// <returns>Count of state</returns>
        public int GetCountOfState(PersonalizationScope scope, PersonalizationStateQuery query)
        {
            return Provider.GetCountOfState(scope, query);
        }

        /// <summary>
        /// Gets the blob of shared personalization data
        /// </summary>
        /// <param name="path">Path to data</param>
        /// <param name="userName">User name</param>
        /// <returns>Shared data blob</returns>
        public byte[] GetSharedPersonalizationBlob(string path, string userName)
        {
            return Provider.GetSharedPersonalizationBlob(path, userName);
        }

        /// <summary>
        /// Gets the blob of user-specific personalization data
        /// </summary>
        /// <param name="path">Path to data</param>
        /// <param name="userName">User name</param>
        /// <returns>User data blob</returns>
        public byte[] GetUserPersonalizationBlob(string path, string userName)
        {
            return Provider.GetUserPersonalizationBlob(path, userName);
        }

        /// <summary>
        /// Resets personalization data
        /// </summary>
        /// <param name="path">Path to data</param>
        /// <param name="userName">User name</param>
        public void ResetPersonalizationBlob(string path, string userName)
        {
            Provider.ResetPersonalizationBlob(path, userName);
        }

        /// <summary>
        /// Resets personalization state
        /// </summary>
        /// <param name="scope">Scope to reset</param>
        /// <param name="paths">Paths to data</param>
        /// <param name="usernames">Usernames to reset</param>
        /// <returns>Count of reset users</returns>
        public int ResetState(PersonalizationScope scope, string[] paths, string[] usernames)
        {
            return Provider.ResetState(scope, paths, usernames);
        }

        /// <summary>
        /// Resets user personalization state
        /// </summary>
        /// <param name="path">Path to data</param>
        /// <param name="userInactiveSinceDate">Date to clear inactive users</param>
        /// <returns>Count of reset users</returns>
        public int ResetUserState(string path, DateTime userInactiveSinceDate)
        {
            return Provider.ResetUserState(path, userInactiveSinceDate);
        }

        /// <summary>
        /// Saves personalization blob
        /// </summary>
        /// <param name="path">Path to data</param>
        /// <param name="userName">User name</param>
        /// <param name="dataBlob">Data blob</param>
        public void SavePersonalizationBlob(string path, string userName, byte[] dataBlob)
        {
            Provider.SavePersonalizationBlob(path, userName, dataBlob);
        }

        #endregion
    }
}
