using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Connections
{
    public class Connection
    {
        private readonly string _connectionString;

        public Connection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public object? ExecuteScalar(Command command)
        {
            throw new NotImplementedException();
        }

        public int ExecuteNonQuery(Command command)
        {
            using (SqlConnection dbConnection = CreateConnection())
            {
                using(SqlCommand dbCommand = CreateCommand(dbConnection, command))
                {
                    dbConnection.Open();
                    return dbCommand.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<TResult> ExecuteReader<TResult>(Command command, Func<IDataRecord, TResult> selector, bool immediately)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TResult> ExecuteReader<TResult>(Command command, Func<IDataRecord, TResult> selector)
        {
            using (SqlConnection dbConnection = CreateConnection())
            {
                using (SqlCommand dbCommand = CreateCommand(dbConnection, command))
                {
                    dbConnection.Open();
                    using(SqlDataReader reader = dbCommand.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            yield return selector(reader);
                        }
                    }
                }
            }
        }

        public DataTable GetDataTable(Command command)
        {
            throw new NotImplementedException();
        }

        private SqlCommand CreateCommand(SqlConnection dbConnection, Command command)
        {
            SqlCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = command.Query;
            if(command.IsStoredProcedure)
                dbCommand.CommandType = CommandType.StoredProcedure;

            foreach (KeyValuePair<string, object> kvp in command.Parameters)
            {
                SqlParameter dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = kvp.Key;
                dbParameter.Value = kvp.Value;
                dbCommand.Parameters.Add(dbParameter);
            }

            return dbCommand;
        }

        private SqlConnection CreateConnection()
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = _connectionString;
            return dbConnection;
        }
    }
}
