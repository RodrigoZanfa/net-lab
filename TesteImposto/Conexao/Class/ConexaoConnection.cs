using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Conexao.Enum;

namespace Conexao.Class
{
    public class ConexaoConnection : ConexaoDataAccess
    {
        //private DbProviderFactory dbProviderFactory;

        private String providerName;
        public String ProviderName
        {
            get { return this.providerName; }
        }

        private String connectionString;
        public String ConnectionString
        {
            get { return this.connectionString; }
        }

        //public ConexaoConnection()
        //{
        //    this.ProviderName = ConexaoSettings.DatabaseProviderName;
        //    this.ConnectionString = ConexaoSettings.DatabaseConnectionString;

        //    this.dbProviderFactory = DbProviderFactories.GetFactory(this.ProviderName);
        //}

        //public ConexaoConnection(String databaseUser, String password)
        //    : this()
        //{
        //    this.ConnectionString = ReplaceParameter("User ID", databaseUser, this.ConnectionString);
        //    this.ConnectionString = ReplaceParameter("Password", password, this.ConnectionString);
        //}

        //public Boolean TestConnection()
        //{
        //    DbConnection dbConnection = this.dbProviderFactory.CreateConnection();
        //    dbConnection.ConnectionString = this.ConnectionString;

        //    try
        //    {
        //        dbConnection.Open();

        //        dbConnection.Close();

        //        this.Success = true;
        //        this.ResultType = ConexaoResultType.ConnectionSuccessful;
        //        this.ResultMessage = String.Empty;
        //    }
        //    catch (Exception e)
        //    {
        //        this.Success = false;
        //        this.ResultType = ConexaoResultType.DbError;
        //        this.ResultMessage = e.Message;
        //    }

        //    return this.Success;
        //}

        //private String ReplaceParameter(String parameter, String value, String configurationString)
        //{
        //    String newValue = parameter + "=" + value + ";";

        //    Int32 parameterPosition = configurationString.IndexOf(parameter);

        //    if (parameterPosition == -1)
        //    {
        //        if (configurationString.EndsWith(";") == false)
        //        {
        //            configurationString += ";";
        //        }

        //        configurationString += newValue;
        //    }
        //    else
        //    {
        //        Int32 commaPosition = configurationString.IndexOf(";", parameterPosition);

        //        String oldValue = configurationString.Substring(parameterPosition, (commaPosition - parameterPosition + 1));

        //        configurationString = configurationString.Replace(oldValue, newValue);
        //    }

        //    return configurationString;
        //}
    }
}
