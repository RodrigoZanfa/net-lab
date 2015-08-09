using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Conexao.Class;
using System.Data;

namespace Conexao.Util
{
    public class ConexaoImageService
    {
        //private DbDataAdapter dbDataAdapter;

        //private String imageField;
        //private Dictionary<String, Object> primaryKey;

        //public ConexaoImageService(String tableName, Dictionary<String, Object> primaryKey, String imageField)
        //    : this(tableName, primaryKey, imageField, null)
        //{

        //}

        //public ConexaoImageService(String tableName, Dictionary<String, Object> primaryKey, String imageField, ConexaoTransaction transaction)
        //{
        //    this.imageField = imageField;
        //    this.primaryKey = primaryKey;

        //    ConexaoCommand command = (transaction == null) ? new ConexaoCommand() : new ConexaoCommand(transaction);

        //    // Preenche command com parâmetro para executar comando "SELECT"
        //    this.SetSelectCommand(command, tableName, primaryKey, imageField);

        //    // Inicializa dbProviderFactory e dbCommand
        //    DbProviderFactory dbProviderFactory = command.GetProviderFactory();

        //    DbCommand dbCommand = command.GetCommand();

        //    // Inicializa dbDataAdapter
        //    this.dbDataAdapter = dbProviderFactory.CreateDataAdapter();
        //    this.dbDataAdapter.SelectCommand = dbCommand;

        //    // Inicializa dbCommandBuilder
        //    DbCommandBuilder dbCommandBuilder = dbProviderFactory.CreateCommandBuilder();
        //    dbCommandBuilder.DataAdapter = this.dbDataAdapter;
        //}

        //private ConexaoCommand SetSelectCommand(ConexaoCommand command, String tableName, Dictionary<String, Object> primaryKey, String imageField)
        //{
        //    StringBuilder sql = new StringBuilder();

        //    String fieldDelimiter = String.Empty;
        //    String clauseAND = String.Empty;

        //    sql.Append("SELECT ");

        //    foreach (String key in this.primaryKey.Keys)
        //    {
        //        sql.Append(fieldDelimiter + key.ToUpper());

        //        fieldDelimiter = ", ";
        //    }

        //    sql.Append(fieldDelimiter + imageField.ToUpper());

        //    sql.Append(" FROM " + tableName.ToUpper());

        //    sql.Append(" WHERE ");

        //    foreach (String key in primaryKey.Keys)
        //    {
        //        sql.Append(clauseAND + key.ToUpper() + " = " + primaryKey[key]);

        //        clauseAND = " AND ";
        //    }

        //    command.CommandText = sql.ToString();

        //    return command;
        //}

        //public Byte[] GetImage()
        //{
        //    Byte[] imageData;

        //    DataTable dataTable = new DataTable();

        //    this.dbDataAdapter.FillSchema(dataTable, SchemaType.Source);
        //    this.dbDataAdapter.Fill(dataTable);

        //    if (dataTable.Rows.Count == 0)
        //    {
        //        imageData = null;
        //    }
        //    else
        //    {
        //        DataRow dataRow = dataTable.Rows[0];

        //        imageData = (Byte[])dataRow[imageField];
        //    }

        //    return imageData;
        //}

        //public void UpdateImage(Byte[] imageData)
        //{
        //    DataTable dataTable = new DataTable();

        //    this.dbDataAdapter.FillSchema(dataTable, SchemaType.Source);
        //    this.dbDataAdapter.Fill(dataTable);

        //    DataRow dataRow = dataTable.Rows.Count == 0 ? dataTable.NewRow() : dataTable.Rows[0];

        //    foreach (String key in this.primaryKey.Keys)
        //    {
        //        dataRow[key] = primaryKey[key];
        //    }

        //    dataRow[imageField] = imageData;

        //    if (dataTable.Rows.Count == 0)
        //    {
        //        dataTable.Rows.Add(dataRow);
        //    }

        //    this.dbDataAdapter.Update(dataTable);
        //}
    }
}
