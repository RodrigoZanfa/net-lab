using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace Conexao.Class
{
    public class ConexaoTransaction
    {
        private DbProviderFactory dbProviderFactory;
        public DbProviderFactory ProviderFactory
        {
            get { return this.dbProviderFactory; }
        }

        private DbConnection dbConnection;
        public DbConnection Connection
        {
            get { return this.dbConnection; }
        }

        private DbTransaction dbTransaction;
        public DbTransaction Transaction
        {
            get { return this.dbTransaction; }
        }

        private DateTime beginTime;

        private TimeSpan elapsedTime;
        public TimeSpan ElapsedTime
        {
            get { return this.elapsedTime; }
            private set { this.elapsedTime = value; }
        }

        public ConexaoTransaction()
        {
            this.beginTime = DateTime.Now;

            this.dbProviderFactory = DbProviderFactories.GetFactory(ConexaoSettings.DatabaseProviderName);

            this.dbConnection = this.dbProviderFactory.CreateConnection();
            this.dbConnection.ConnectionString = ConexaoSettings.DatabaseConnectionString;

            this.dbConnection.Open();

            this.dbTransaction = this.dbConnection.BeginTransaction();
        }

        public void Commit()
        {
            this.dbTransaction.Commit();

            this.dbConnection.Close();

            this.ElapsedTime = DateTime.Now - this.beginTime;

            this.dbTransaction = null;
        }

        public void RollBack()
        {
            this.dbTransaction.Rollback();

            this.dbConnection.Close();

            this.ElapsedTime = DateTime.Now - this.beginTime;

            this.dbTransaction = null;
        }
    }
}
