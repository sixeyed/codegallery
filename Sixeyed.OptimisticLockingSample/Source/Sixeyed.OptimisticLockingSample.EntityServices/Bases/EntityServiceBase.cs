using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sixeyed.OptimisticLockingSample.EntityServices.Interfaces;
using System.Data.SqlClient;
using System.Configuration;

namespace Sixeyed.OptimisticLockingSample.EntityServices.Bases
{
    public abstract class EntityServiceBase<TEntity> : IEntityService<TEntity>
    {
        public Type EntityType
        {
            get { return typeof(TEntity); }
        }

        public abstract TEntity Load(TEntity entity);

        public abstract void Update(TEntity entity);

        private SqlConnection _connection;
        protected SqlConnection Connection 
        {
            get 
            {
                if (this._connection == null)
                {
                    this._connection = new SqlConnection(ConfigurationManager.ConnectionStrings["OLSample"].ConnectionString);
                    this._connection.Open();
                }
                return this._connection;
            }
        }
    }
}
