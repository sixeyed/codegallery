using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace ExcelUpload.Data.Bases
{
    /// <summary>
    /// Base class providing functionality to execute a stored procedure
    /// </summary>
    /// <typeparam name="TConnection">Type of IDbConnection</typeparam>
    /// <typeparam name="TDataAdapter">Type of IDbDataAdapter</typeparam>
    /// <typeparam name="TParameter">Type of IDataParameter</typeparam>    
    public abstract class StoredProcedureBase<TConnection, TDataAdapter, TParameter> : QueryBase<TConnection, TDataAdapter> 
        where TConnection : class, IDbConnection, new()
        where TDataAdapter : IDbDataAdapter, new()
        where TParameter : IDataParameter
    {
        private bool _createdParameters;
        private Dictionary<string, TParameter> _parameters = new Dictionary<string, TParameter>();

        #region Abstract instance members

        /// <summary>
        /// Name of the stored procedure to execute
        /// </summary>
        public abstract string SPName { get;}

        /// <summary>
        /// Creates parameters for the call
        /// </summary>
        protected abstract void CreateParameters();

        /// <summary>
        /// Sets the input values of the procedure parameters
        /// </summary>
        protected abstract void SetInputParameterValues();

        /// <summary>
        /// Sets the output values from the procedure parameters
        /// </summary>
        protected abstract void GetOutputParameterValues();

        #endregion

        #region Protected instance properties

        /// <summary>
        /// List of parameters to the stored procedure
        /// </summary>
        protected Dictionary<string, TParameter> Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        /// Returns stored procedure command type
        /// </summary>
        protected override CommandType CommandType
        {
            get { return CommandType.StoredProcedure; }
        }

        /// <summary>
        /// Returns default command behaviour
        /// </summary>
        protected override CommandBehavior CommandBehavior
        {
            get { return CommandBehavior.Default; }
        }

        /// <summary>
        /// Gets the name of the stored procedure to execute
        /// </summary>
        protected override string CommandText
        {
            get { return this.SPName; }
        }

        #endregion

        #region Protected instance methods

        /// <summary>
        /// Map output parameter values after call
        /// </summary>
        protected override void AfterExecute()
        {
            this.GetOutputParameterValues();
        }

        /// <summary>
        /// Builds the command to be executed
        /// </summary>
        /// <returns>Prepared IDbCommand</returns>
        protected override IDbCommand PrepareCommand()
        {
            IDbCommand command = base.PrepareCommand();
            //add parameters:
            if (!_createdParameters)
            {
                this.CreateParameters();
                _createdParameters = true;
            }
            foreach (IDataParameter parameter in this.Parameters.Values)
            {
                command.Parameters.Add(parameter);
            }
            if (this.Parameters.Count > 0)
            {
                this.SetInputParameterValues();
            }
            return command;
        }

        #endregion
    }
}
