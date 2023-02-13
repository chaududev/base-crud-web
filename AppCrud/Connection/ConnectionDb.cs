using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppCrud.Connection
{
    public class ConnectionDb 
    {
      public readonly IConfiguration _config;
        SqlConnection _conn;
        public ConnectionDb(IConfiguration config)
        {
            this._config = config;
        }
       public SqlConnection GetConnection()
        {
            try
            {
                _conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
                return _conn;
            }
            catch(Exception ex)
            {
                throw;
            }
             
        }

    }
}
