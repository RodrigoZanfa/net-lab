using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Conexao.Class;
using Conexao.Enum;
using Imposto.Core.Domain;

namespace Imposto.Core.Data
{
    public class NotaFiscalRepository : ConexaoDAO
    {
        public void SalvarNotaFiscal(int Id, int NumeroNotaFiscal, int Serie, string NomeCliente, string EstadoDestino, string EstadoOrigem, List<NotaFiscalItem> ItensDaNotaFiscal)
        {
            ConexaoTransaction transaction = new ConexaoTransaction();

            this.SalvarTabelaNotaFiscal(ref Id, NumeroNotaFiscal, Serie, NomeCliente, EstadoDestino, EstadoOrigem, transaction);

            if (this.ResultType != ConexaoResultType.DbError)
            {
                foreach (NotaFiscalItem itemNotaFiscal in ItensDaNotaFiscal)
                {
                    // corrigindo o ID da Nota Fiscal que foi gerado, para ser referenciado em seus itens
                    itemNotaFiscal.IdNotaFiscal = Id;

                    this.SalvarTabelaNotaFiscalItem(itemNotaFiscal, transaction);
                }
            }

            if (this.ResultType != ConexaoResultType.DbError)
            {
                transaction.Commit();
            }
            else
            {
                transaction.RollBack();
            }
        }

        private void SalvarTabelaNotaFiscal(ref int Id, int NumeroNotaFiscal, int Serie, string NomeCliente, string EstadoDestino, string EstadoOrigem, ConexaoTransaction transaction)
        {
            ConexaoCommand command = (transaction == null) ? new ConexaoCommand() : new ConexaoCommand(transaction);

            StringBuilder sql = new StringBuilder();
            sql.Append("P_NOTA_FISCAL ");

            command.CommandText = sql.ToString();
            command.CommandType = CommandType.StoredProcedure;

            command.SetParamWithValue("pId", Id, ParameterDirection.InputOutput, DbType.Int32);
            command.SetParamWithValue("pNumeroNotaFiscal", NumeroNotaFiscal);
            command.SetParamWithValue("pSerie", Serie);
            command.SetParamWithValue("pNomeCliente", NomeCliente);
            command.SetParamWithValue("pEstadoDestino", EstadoDestino);
            command.SetParamWithValue("pEstadoOrigem", EstadoOrigem);

            command.ExecuteNonQuery();

            this.SetResult(command);

            if (this.Success)
            {
                Id = Convert.ToInt32(command.GetParamValue("pId"));
            }

            command.Dispose();
        }

        private void SalvarTabelaNotaFiscalItem(NotaFiscalItem itemNotaFiscal, ConexaoTransaction transaction)
        {
            ConexaoCommand command = (transaction == null) ? new ConexaoCommand() : new ConexaoCommand(transaction);

            StringBuilder sql = new StringBuilder();
            sql.Append("P_NOTA_FISCAL_ITEM ");

            command.CommandText = sql.ToString();
            command.CommandType = CommandType.StoredProcedure;

            command.SetParamWithValue("pId", itemNotaFiscal.Id);
            command.SetParamWithValue("pIdNotaFiscal", itemNotaFiscal.IdNotaFiscal);
            command.SetParamWithValue("pCfop", itemNotaFiscal.Cfop);
            command.SetParamWithValue("pTipoIcms", itemNotaFiscal.TipoIcms);
            command.SetParamWithValue("pBaseIcms", itemNotaFiscal.BaseIcms);
            command.SetParamWithValue("pAliquotaIcms", itemNotaFiscal.AliquotaIcms);
            command.SetParamWithValue("pValorIcms", itemNotaFiscal.ValorIcms);
            command.SetParamWithValue("pNomeProduto", itemNotaFiscal.NomeProduto);
            command.SetParamWithValue("pCodigoProduto", itemNotaFiscal.CodigoProduto);
            command.SetParamWithValue("pBaseIpi", itemNotaFiscal.BaseIpi);
            command.SetParamWithValue("pAliquotaIpi", itemNotaFiscal.AliquotaIpi);
            command.SetParamWithValue("pValorIpi", itemNotaFiscal.ValorIpi);
            command.SetParamWithValue("pPercDesconto", itemNotaFiscal.PercDesconto);

            command.ExecuteNonQuery();

            this.SetResult(command);

            command.Dispose();
        }
    }
}
