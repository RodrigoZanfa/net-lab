using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Conexao.Interface;
using Conexao.Enum;
using System.Data;

namespace Conexao.Class
{
    public class ConexaoSequence : ConexaoDataAccess
    {
        private ConexaoCommand command;

        public ConexaoSequence(String sequence)
        {
            this.command = new ConexaoCommand();

            this.command.CommandText = "SELECT " + sequence + ".NEXTVAL VALOR FROM DUAL";
        }

        public Int64 GetNextValue()
        {
            Int64 valor = 0;

            //DataRow dataRow = this.command.GetFirstDataRow();

            //if (dataRow != null)
            //{
            //    valor = Convert.ToInt64(dataRow["VALOR"]);
            //}

            valor = Convert.ToInt64(this.command.ExecuteScalar());

            this.SetResult(this.command);

            this.command.Dispose();

            return valor;
        }
    }
}
