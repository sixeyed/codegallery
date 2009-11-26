using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;
using ExcelUpload.Data.Interfaces;

namespace ExcelUpload.Data.Bases
{
    /// <summary>
    /// Base class representing a transaction on a database connection
    /// </summary>
    public abstract class TransactionBase<TConnection> : DatabaseClient<TConnection>, IDisposable, IDbTransaction
        where TConnection : class, IDbConnection, new()
    {
        #region Private static members

        private static TransactionBase<TConnection> CurrentInternal
        {
            get
            {
                //get the transaction from the current thread - null if none:
                LocalDataStoreSlot threadSlot = Thread.GetNamedDataSlot(TransactionBase<TConnection>.ThreadLocalStore.Name);
                return Thread.GetData(threadSlot) as TransactionBase<TConnection>;
            }
        }

        #endregion

        #region Public static members

        /// <summary>
        /// Gets the currently executing transaction - null if no transaction in scope
        /// </summary>
        public static IDbTransaction Current
        {
            get
            {
                IDbTransaction transaction = null;
                TransactionBase<TConnection> tx = CurrentInternal;
                if (tx != null && tx._transaction != null)
                {
                    transaction = tx._transaction;
                }
                return transaction;
            }
        }

        /// <summary>
        /// Gets a typed representaion of the executing transaction
        /// </summary>
        /// <typeparam name="TTransaction">Type to return</typeparam>
        /// <returns>Current transaction, null if no transaction in scope</returns>
        public static TTransaction GetCurrent<TTransaction>()
            where TTransaction : class, IDbTransaction
        {
            TTransaction current = default(TTransaction);
            if (TransactionBase<TConnection>.Current != null)
            {
                current = TransactionBase<TConnection>.Current as TTransaction;
            }
            return current;
        }

        /// <summary>
        /// Returns whether the given client is currently inside a transaction scope
        /// </summary>
        /// <param name="client">Client to check</param>
        /// <returns>Whether the database is in a transaction</returns>
        public static bool InTransactionScope(IDatabaseClient client)
        {
            return InTransactionScope(client.DatabaseName);
        }

        /// <summary>
        /// Returns whether the given database is currently inside a transaction scope
        /// </summary>
        /// <param name="databaseName">Database name to check</param>
        /// <returns>Whether the database is in a transaction</returns>
        public static bool InTransactionScope(string databaseName)
        {
            TransactionBase<TConnection> transaction = CurrentInternal;
            return (transaction != null && transaction.DatabaseName == databaseName);
        }

        #endregion

        #region Private instance members

        private IDbTransaction _transaction;

        private void DisposeTransaction()
        {
            this._transaction.Dispose();
            this._transaction = null;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor - starts the transaction
        /// </summary>
        protected TransactionBase()
        {
            this.Start();
        }

        #endregion

        #region Public instance members

        /// <summary>
        /// Starts a transaction for the database connection
        /// </summary>
        public void Start()
        {
            if (this._transaction == null)
            {
                //begin the transaction:
                this._transaction = this.Connection.BeginTransaction();
                //add the transaction to TLS so the connection can be shared with other clients in scope:
                LocalDataStoreSlot threadSlot = Thread.GetNamedDataSlot(ThreadLocalStore.Name);
                Thread.SetData(threadSlot, this);
            }
        }

        /// <summary>
        /// Commits the transaction
        /// </summary>
        public void Commit()
        {
            if (this._transaction != null)
            {
                this._transaction.Commit();
                this.DisposeTransaction();
            }
        }


        /// <summary>
        /// Rolls back the transaction
        /// </summary>
        public void Rollback()
        {
            if (this._transaction != null)
            {
                this._transaction.Rollback();
                DisposeTransaction();
            }
        }

#endregion

        #region IDbTransaction Members

        /// <summary>
        /// Returns the connection used by the transaction
        /// </summary>
        public new IDbConnection Connection
        {
            get 
            {
                IDbConnection connection = null;
                if (this._transaction != null)
                {
                    connection = this._transaction.Connection;
                }
                else
                {
                    connection = base.Connection;
                }
                return connection;
            }
        }

        /// <summary>
        /// Returns the isolation level of the transaction
        /// </summary>
        public IsolationLevel IsolationLevel
        {
            get { return this._transaction.IsolationLevel; }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Cleans up database resources
        /// </summary>
        /// <param name="disposing">Whether disposing</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //if the transaction is still open, we haven't been
                //committed, so roll it back:
                if (this._transaction != null)
                {
                    this.Rollback();
                }
                //clear up the thread store:
                Thread.FreeNamedDataSlot(ThreadLocalStore.Name);
                //and dispose the connection:
                base.Dispose(true);
            }
        }

        #endregion

        #region Public structs

        /// <summary>
        /// Struct for known thread local store names
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes",
            Justification="Struct used only for defining constants")]
        public struct ThreadLocalStore
        {
            /// <summary>
            /// "ExcelUpload.Data.Bases.Transaction._transaction"
            /// </summary>
            public const string Name = "ExcelUpload.Data.Bases.Transaction._transaction";
        }

        #endregion
    }
}
