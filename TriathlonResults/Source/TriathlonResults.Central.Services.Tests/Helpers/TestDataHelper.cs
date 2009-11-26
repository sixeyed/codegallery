using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace TriathlonResults.Central.Services.Tests.Helpers
{
    public static class TestDataHelper
    {
        public static T GetScalar<T>(string sqlCommand)
        {
            T typedResult = default(T);
            using (SqlConnection connection = new SqlConnection(@"Data Source=BIZTEST\DEV01;Initial Catalog=TriathlonResults;User ID=TriathlonResults;Password=TriathlonResults"))
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = sqlCommand;
                object result = command.ExecuteScalar();
                try
                {
                    typedResult = (T)result;
                }
                catch (Exception ex)
                {
                }
            }
            return typedResult;
        }
    }
}
