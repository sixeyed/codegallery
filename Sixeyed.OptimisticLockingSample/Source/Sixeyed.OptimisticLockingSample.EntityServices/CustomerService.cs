using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sixeyed.OptimisticLockingSample.Entities;
using Sixeyed.OptimisticLockingSample.EntityServices.Interfaces;
using System.Data.SqlClient;
using Sixeyed.OptimisticLockingSample.EntityServices.Bases;
using System.Data;
using System.IO;

namespace Sixeyed.OptimisticLockingSample.EntityServices
{
    public class CustomerService : EntityServiceBase<Customer>
    {
        public override Customer Load(Customer entity)
        {
            Customer loadedEntity = new Customer();
            SqlCommand getCommand = new SqlCommand("GetCustomer", this.Connection);
            getCommand.CommandType = CommandType.StoredProcedure;
            getCommand.Parameters.Add("@Id", SqlDbType.Int);

            getCommand.Parameters["@Id"].Value = entity.Id;
            using (SqlDataReader reader = getCommand.ExecuteReader(CommandBehavior.SingleRow))
            {
                if (reader.Read())
                {
                    loadedEntity.Id = (int)reader["Id"];
                    loadedEntity.Name = (string)reader["Name"];
                    loadedEntity.StartDate = (DateTime)reader["StartDate"];
                    loadedEntity.CreditLimit = Convert.ToSingle(reader["CreditLimit"]);
                    loadedEntity.Logo = (byte[])reader["Logo"];
                }
            }

            return loadedEntity;
        }

        public override void Update(Customer entity)
        {
            SqlCommand setCommand = new SqlCommand("SetCustomer", this.Connection);
            setCommand.CommandType = CommandType.StoredProcedure;
            setCommand.Parameters.Add("@Id", SqlDbType.Int);
            setCommand.Parameters.Add("@Name", SqlDbType.NVarChar, 50);
            setCommand.Parameters.Add("@StartDate", SqlDbType.DateTime);
            setCommand.Parameters.Add("@CreditLimit", SqlDbType.Float);
            setCommand.Parameters.Add("@Logo", SqlDbType.Image);

            setCommand.Parameters["@Id"].Value = entity.Id;
            setCommand.Parameters["@Name"].Value = entity.Name;
            setCommand.Parameters["@StartDate"].Value = entity.StartDate;
            setCommand.Parameters["@CreditLimit"].Value = entity.CreditLimit;
            setCommand.Parameters["@Logo"].Value = entity.Logo;

            setCommand.ExecuteNonQuery();
        }
    }
}
