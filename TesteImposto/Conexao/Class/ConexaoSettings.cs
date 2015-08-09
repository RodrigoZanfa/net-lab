using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Conexao.Class
{
    public class ConexaoSettings
    {
        //private static String databaseProviderName;
        //private static String databaseConnectionString;

        //public static String GetDatabaseProviderName()
        //{
        //    return databaseProviderName;
        //}

        //public static void SetDatabaseProviderName(String value)
        //{
        //    databaseProviderName = value;
        //}

        //public static String GetDatabaseConnectionString()
        //{
        //    return databaseConnectionString;
        //}

        //public static void SetDatabaseConnectionString(String value)
        //{
        //    databaseConnectionString = value;
        //}

        private static String databaseProviderName;
        public static String DatabaseProviderName
        {
            get { return databaseProviderName; }
            set { databaseProviderName = value; }
        }

        private static String databaseConnectionString;
        public static String DatabaseConnectionString
        {
            get { return databaseConnectionString; }
            set { databaseConnectionString = value; }
        }

        static ConexaoSettings()
        {
            ConnectionStringSettings databaseConnectionString = ConfigurationManager.ConnectionStrings["SQLSERVER"];

            //SetDatabaseProviderName(databaseConnectionString.ProviderName);
            //SetDatabaseConnectionString(databaseConnectionString.ConnectionString);

            DatabaseProviderName = databaseConnectionString.ProviderName;
            DatabaseConnectionString = databaseConnectionString.ConnectionString;
        }
    }
}
