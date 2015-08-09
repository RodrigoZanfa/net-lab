using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Conexao.Interface;
using System.Data.Common;
using System.Data;
using Conexao.Enum;

namespace Conexao.Class
{
    public class ConexaoCommand : ConexaoDataAccess, IDisposable
    {
        private readonly String constMsgSuccessfullTransaction = "Transação realizada com sucesso.";
        private readonly String constMsgErrorTransaction = "Ocorreu um erro durante a transação no banco de dados - ";
        private readonly String constMsgDataFound = "Dados encontrados.";
        private readonly String constMsgNoDataFound = "Dados não encontrados.";
        private readonly String constMsgQueryError = "Ocorreu um erro durante a consulta no banco de dados - ";

        private DbProviderFactory dbProviderFactory;
        //public DbProviderFactory ProviderFactory  // GetProviderFactory()
        //{
        //    get { return this.dbProviderFactory; }  // return this.dbProviderFactory;
        //}

        private DbConnection dbConnection;
        //public DbConnection Connection  // GetConnection()
        //{
        //    get { return this.dbConnection; }  // return this.dbConnection;
        //}

        private DbCommand dbCommand;
        //public DbCommand Command  // GetCommand()
        //{
        //    get { return this.dbCommand; }  // return this.dbCommand;
        //}

        private DbDataReader dbDataReader;
        public DbDataReader DataReader  // GetDataReader()
        {
            get { return this.dbDataReader; }  // return this.dbDataReader;
        }

        //public CommandType SetCommandType
        //{
        //    set { this.dbCommand.CommandType = value; }
        //}

        public String CommandText
        {
            get { return this.dbCommand.CommandText; }
            set { this.dbCommand.CommandText = value; }
        }

        public CommandType CommandType
        {
            get { return this.dbCommand.CommandType; }
            set { this.dbCommand.CommandType = value; }
        }

        private DateTime beginTime;

        private TimeSpan elapsedTime;
        public TimeSpan ElapsedTime
        {
            get { return this.elapsedTime; }
            private set { this.elapsedTime = value; }
        }

        public ConexaoCommand()
        {
            CreateNewDbCommand(null, null);
        }

        public ConexaoCommand(ConexaoTransaction transaction)
        {
            CreateNewDbCommand(null, transaction);
        }

        //public ConexaoCommand(ConexaoConnection connection)
        //{
        //    CreateNewDbCommand(connection, null);
        //}

        private void CreateNewDbCommand(ConexaoConnection connection, ConexaoTransaction transaction)
        {
            if (connection != null)
            {
                this.dbProviderFactory = DbProviderFactories.GetFactory(connection.ProviderName);

                this.dbConnection = this.dbProviderFactory.CreateConnection();
                this.dbConnection.ConnectionString = connection.ConnectionString;

                this.dbCommand = this.dbProviderFactory.CreateCommand();
                this.dbCommand.Connection = this.dbConnection;

                this.dbDataReader = null;
            }
            else if (transaction != null)
            {
                this.dbProviderFactory = transaction.ProviderFactory;

                this.dbConnection = transaction.Connection;

                this.dbCommand = this.dbProviderFactory.CreateCommand();
                this.dbCommand.Connection = this.dbConnection;
                this.dbCommand.Transaction = transaction.Transaction;
            }
            else
            {
                this.dbProviderFactory = DbProviderFactories.GetFactory(ConexaoSettings.DatabaseProviderName);

                this.dbConnection = this.dbProviderFactory.CreateConnection();
                this.dbConnection.ConnectionString = ConexaoSettings.DatabaseConnectionString;

                this.dbCommand = this.dbProviderFactory.CreateCommand();
                this.dbCommand.Connection = this.dbConnection;

                this.dbDataReader = null;
            }
        }

        private void OpenConnection()
        {
            // A conexão deve ser aberta se dbCommand não for parte de uma transação
            if (this.dbCommand.Transaction == null)
            {
                // Abre a conexão com o banco de dados
                this.dbConnection.Open();
            }
        }

        private void CloseConnection()
        {
            // A conexão deve ser fechada se dbCommand não for parte de uma transação
            if (this.dbCommand.Transaction == null)
            {
                // Fecha a conexão com o banco de dados
                this.dbConnection.Close();
            }
        }

        #region Executa comandos no banco de dados (INSERT, UPDATE, DELETE)
        public Int32 ExecuteNonQuery()
        {
            this.beginTime = DateTime.Now;

            Int32 rowsAffected;

            OpenConnection();

            try
            {
                rowsAffected = this.dbCommand.ExecuteNonQuery();

                this.Success = true;
                this.ResultType = rowsAffected > 0 ? ConexaoResultType.RowsAffected : ConexaoResultType.NoRowsAffected;
                this.ResultMessage = this.constMsgSuccessfullTransaction;
            }
            catch (DbException e)
            {
                rowsAffected = 0;

                this.Success = false;
                this.ResultType = ConexaoResultType.DbError;
                this.ResultMessage = this.constMsgErrorTransaction + e.Message;
            }
            finally
            {
                CloseConnection();
            }

            this.ElapsedTime = DateTime.Now - this.beginTime;

            return rowsAffected;
        }
        #endregion

        #region Executa consulta (SELECT) no banco de dados
        public Boolean ExecuteReader()
        {
            return ExecuteReader(CommandBehavior.CloseConnection);
        }

        public Boolean ExecuteReader(CommandBehavior commandBehavior)
        {
            this.beginTime = DateTime.Now;

            OpenConnection();

            try
            {
                if (this.dbCommand.Transaction == null)
                {
                    this.dbDataReader = this.dbCommand.ExecuteReader(commandBehavior);
                }
                else
                {
                    this.dbDataReader = this.dbCommand.ExecuteReader();
                }

                if (this.dbDataReader.HasRows)
                {
                    this.Success = true;
                    this.ResultType = ConexaoResultType.DataFound;
                    this.ResultMessage = this.constMsgDataFound;
                }
                else
                {
                    this.Success = false;
                    this.ResultType = ConexaoResultType.NoDataFound;
                    this.ResultMessage = this.constMsgNoDataFound;
                }
            }
            catch (DbException e)
            {
                this.Success = false;
                this.ResultType = ConexaoResultType.DbError;
                this.ResultMessage = this.constMsgQueryError + e.Message;
            }
            //finally
            //{
            //    CloseConnection();
            //}

            this.ElapsedTime = DateTime.Now - this.beginTime;

            return this.Success;
        }
        #endregion

        #region Executa consulta (SELECT) no banco de dados, retornando apenas 1 linha com 1 coluna
        public Object ExecuteScalar()
        {
            this.beginTime = DateTime.Now;

            Object resultado;

            OpenConnection();

            try
            {
                resultado = this.dbCommand.ExecuteScalar();

                if (resultado != null)
                {
                    this.Success = true;
                    this.ResultType = ConexaoResultType.DataFound;
                    this.ResultMessage = this.constMsgDataFound;
                }
                else
                {
                    this.Success = false;
                    this.ResultType = ConexaoResultType.NoDataFound;
                    this.ResultMessage = this.constMsgNoDataFound;
                }
            }
            catch (DbException e)
            {
                resultado = null;

                this.Success = false;
                this.ResultType = ConexaoResultType.DbError;
                this.ResultMessage = this.constMsgQueryError + e.Message;
            }
            finally
            {
                CloseConnection();
            }

            this.ElapsedTime = DateTime.Now - this.beginTime;

            return resultado;
        }
        #endregion

        #region Executa consulta (SELECT) no banco de dados, retornando através de um DataTable
        public DataTable GetDataTable()
        {
            this.beginTime = DateTime.Now;

            DataTable dataTable = new DataTable();

            DbDataAdapter dbDataAdapter = this.dbProviderFactory.CreateDataAdapter();
            dbDataAdapter.SelectCommand = this.dbCommand;

            try
            {
                dbDataAdapter.Fill(dataTable);

                this.Success = (dataTable.Rows.Count > 0);
                this.ResultType = this.Success ? ConexaoResultType.DataFound : ConexaoResultType.NoDataFound;
                this.ResultMessage = this.Success ? this.constMsgDataFound : this.constMsgNoDataFound;
            }
            catch (DbException e)
            {
                this.Success = false;
                this.ResultType = ConexaoResultType.DbError;
                this.ResultMessage = this.constMsgQueryError + e.Message;
            }
            finally
            {
                dbDataAdapter = null;
            }

            this.ElapsedTime = DateTime.Now - this.beginTime;

            return dataTable;
        }

        //public DataTable GetDataTable(Boolean habilitarFillSchema)
        //{
        //    this.beginTime = DateTime.Now;

        //    DataTable dataTable = new DataTable();

        //    DbDataAdapter dbDataAdapter = this.dbProviderFactory.CreateDataAdapter();
        //    dbDataAdapter.SelectCommand = this.dbCommand;

        //    try
        //    {
        //        if (habilitarFillSchema)
        //        {
        //            dbDataAdapter.FillSchema(dataTable, SchemaType.Source);
        //        }

        //        dbDataAdapter.Fill(dataTable);

        //        this.Success = (dataTable.Rows.Count > 0);
        //        this.ResultType = this.Success ? ConexaoResultType.DataFound : ConexaoResultType.NoDataFound;
        //        this.ResultMessage = this.Success ? this.constMsgDataFound : this.constMsgNoDataFound;
        //    }
        //    catch (DbException e)
        //    {
        //        this.Success = false;
        //        this.ResultType = ConexaoResultType.DbError;
        //        this.ResultMessage = this.constMsgQueryError + e.Message;
        //    }
        //    finally
        //    {
        //        dbDataAdapter = null;
        //    }

        //    this.ElapsedTime = DateTime.Now - this.beginTime;

        //    return dataTable;
        //}

        //public DataTable GetDataTable(Int32 maxRecords)
        //{
        //    this.beginTime = DateTime.Now;

        //    DataTable[] dataTableList = new DataTable[] { new DataTable() };

        //    DataTable dataTable = new DataTable();

        //    DbDataAdapter dbDataAdapter = this.dbProviderFactory.CreateDataAdapter();
        //    dbDataAdapter.SelectCommand = this.dbCommand;

        //    try
        //    {
        //        dbDataAdapter.Fill(0, maxRecords, dataTableList);

        //        dataTable = dataTableList[0];

        //        this.Success = (dataTable.Rows.Count > 0);
        //        this.ResultType = this.Success ? ConexaoResultType.DataFound : ConexaoResultType.NoDataFound;
        //        this.ResultMessage = this.Success ? this.constMsgDataFound : this.constMsgNoDataFound;
        //    }
        //    catch (DbException e)
        //    {
        //        this.Success = false;
        //        this.ResultType = ConexaoResultType.DbError;
        //        this.ResultMessage = this.constMsgQueryError + e.Message;
        //    }
        //    finally
        //    {
        //        dbDataAdapter = null;
        //    }

        //    this.ElapsedTime = DateTime.Now - this.beginTime;

        //    return dataTable;
        //}
        #endregion

        //public Boolean HasRows()
        //{
        //    this.beginTime = DateTime.Now;

        //    OpenConnection();

        //    try
        //    {
        //        Object value = this.dbCommand.ExecuteScalar();

        //        this.Success = (value != null);
        //        this.ResultType = this.Success ? ConexaoResultType.DataFound : ConexaoResultType.NoDataFound;
        //        this.ResultMessage = this.Success ? this.constMsgDataFound : this.constMsgNoDataFound;
        //    }
        //    catch (DbException e)
        //    {
        //        this.Success = false;
        //        this.ResultType = ConexaoResultType.DbError;
        //        this.ResultMessage = this.constMsgErrorTransaction + e.Message;
        //    }
        //    finally
        //    {
        //        CloseConnection();
        //    }

        //    this.ElapsedTime = DateTime.Now - this.beginTime;

        //    return this.Success;
        //}

        //public DataRow GetFirstDataRow()
        //{
        //    this.beginTime = DateTime.Now;

        //    DataTable dataTable = this.GetDataTable();

        //    DataRow dataRow = null;

        //    if (dataTable.Rows.Count > 0)
        //    {
        //        dataRow = dataTable.Rows[0];
        //    }

        //    this.ElapsedTime = DateTime.Now - this.beginTime;

        //    return dataRow;
        //}

        #region Cria ou altera um parâmetro do DbCommand
        public DbParameter SetParamWithValue(String parameterName, Object value)
        {
            return SetParamWithValue(parameterName, value, ParameterDirection.Input, null);
        }

        public DbParameter SetParamWithValue(String parameterName, Object value, ParameterDirection? parameterDirection, DbType? dbType)
        {
            DbParameter dbParameter;

            if (value == null)
            {
                value = DBNull.Value;
            }

            if (this.dbCommand.Parameters.Contains(parameterName))
            {
                dbParameter = this.dbCommand.Parameters[parameterName];
                if (parameterDirection.HasValue)
                {
                    dbParameter.Direction = parameterDirection.Value;
                }
                if (dbType.HasValue)
                {
                    dbParameter.DbType = dbType.Value;
                }
                dbParameter.Value = value;
                dbParameter.SourceColumnNullMapping = true;
            }
            else
            {
                dbParameter = this.dbProviderFactory.CreateParameter();
                dbParameter.ParameterName = parameterName;
                if (parameterDirection.HasValue)
                {
                    dbParameter.Direction = parameterDirection.Value;
                }
                if (dbType.HasValue)
                {
                    dbParameter.DbType = dbType.Value;
                }
                dbParameter.Value = value;
                dbParameter.SourceColumnNullMapping = true;

                this.dbCommand.Parameters.Add(dbParameter);
            }

            return dbParameter;
        }

        public Object GetParamValue(String parameterName)
        {
            DbParameter dbParameter = null;

            if (this.dbCommand.Parameters.Contains(parameterName))
            {
                dbParameter = this.dbCommand.Parameters[parameterName];
            }

            return dbParameter.Value;
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            CloseConnection();
        }
        #endregion
    }
}
