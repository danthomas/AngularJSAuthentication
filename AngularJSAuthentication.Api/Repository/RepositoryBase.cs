using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace AngularJSAuthentication.Api.Repository
{
    public abstract class RepositoryBase
    {
        private readonly string _connectionStringName;
        
        protected RepositoryBase(string connectionStringName)
        {
            _connectionStringName = connectionStringName;
        }

        public IDbConnection OpenConnection()
        {
            return OpenConnection(_connectionStringName);
        }

        public IDbConnection OpenConnection(string connectionStringName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

            var sqlConnection = new SqlConnection(connectionString);

            sqlConnection.Open();

            return sqlConnection;
        }

        public void Execute(string query, object @params)
        {
            InConnection((connection, transaction) =>
            {
                connection.Execute(query, @params, transaction);
            });
        }

        public T ExecuteSingleItem<T>(string query, object @params)
        {
            return InConnection((connection, transaction) => connection.Query<T>(query, @params, transaction).SingleOrDefault());
        }

        public List<T> ExecuteList<T>(string query, object @params = null)
        {
            return InConnection((connection, transaction) => connection.Query<T>(query, @params, transaction).ToList());
        }

        public List<T> ExecuteListScalars<T>(string query, object @params = null)
        {
            return InConnection((connection, transaction) =>
            {
                List<T> items = new List<T>();

                var reader = connection.ExecuteReader(query, @params, transaction);

                while (reader.Read())
                {
                    items.Add((T)reader.GetValue(0));
                }

                return items;
            });

        }

        private void InConnection(Action<IDbConnection, IDbTransaction> action)
        {
            using (IDbConnection connection = OpenConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    action(connection, transaction);

                    transaction.Commit();
                }
            }
        }

        private T InConnection<T>(Func<IDbConnection, IDbTransaction, T> action)
        {
            T t = default(T);

            using (IDbConnection connection = OpenConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    t = action(connection, transaction);

                    transaction.Commit();
                }
            }

            return t;
        }
    }
}