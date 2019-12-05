using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Alterview.Infrastructure.Data
{
    public abstract class RepositoryBase
    {
        private readonly string _connectionString;

        public RepositoryBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public abstract int InitializeSchema();
    }
}
