using System;
using System.Collections.Generic;
using System.Text;

namespace View_pay_slip
{
    internal class Dbconnection
    {

        public string serverDbName { get; private set; }
        public string serverName { get; private set; }
        public string serverUserName { get; private set; }
        public string serverPassword { get; private set; }

        public Dbconnection()
        {
            serverDbName = "src_db";
            serverName = "192.168.0.116";
            serverUserName = "sa";
            serverPassword = "masterfile";
        }

        public string sqlConnection()// Call this method to perform sql connection
        {
           return $"Data Source={serverName};Initial Catalog={serverDbName};User ID={serverUserName};Password={serverPassword}";
        }

    }
}
