using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using Imposto.Core.Domain;
using Imposto.Core.Data;
using Conexao.Enum;

namespace Imposto.Core.Service
{
    public class NotaFiscalService
    {
        public void GerarNotaFiscal(Domain.Pedido pedido)
        {
            NotaFiscal notaFiscal = new NotaFiscal();
            notaFiscal.NumeroNotaFiscal = 99999;
            notaFiscal.Serie = new Random().Next(Int32.MaxValue);
            notaFiscal.NomeCliente = pedido.NomeCliente;

            notaFiscal.EstadoDestino = pedido.EstadoDestino;  // pedido.EstadoOrigem;
            notaFiscal.EstadoOrigem = pedido.EstadoOrigem;  // pedido.EstadoDestino;

            foreach (PedidoItem itemPedido in pedido.ItensDoPedido)
            {
                NotaFiscalItem notaFiscalItem = new NotaFiscalItem();
                notaFiscalItem.Cfop = RetornarCfop(notaFiscal);
                if (notaFiscal.EstadoDestino == notaFiscal.EstadoOrigem)
                {
                    notaFiscalItem.TipoIcms = "60";
                    notaFiscalItem.AliquotaIcms = 0.18;
                }
                else
                {
                    notaFiscalItem.TipoIcms = "10";
                    notaFiscalItem.AliquotaIcms = 0.17;
                }
                if (notaFiscalItem.Cfop == "6.009")
                {
                    notaFiscalItem.BaseIcms = itemPedido.ValorItemPedido * 0.90; //redução de base
                }
                else
                {
                    notaFiscalItem.BaseIcms = itemPedido.ValorItemPedido;
                }
                notaFiscalItem.ValorIcms = notaFiscalItem.BaseIcms * notaFiscalItem.AliquotaIcms;

                if (itemPedido.Brinde)
                {
                    notaFiscalItem.TipoIcms = "60";
                    notaFiscalItem.AliquotaIcms = 0.18;
                    notaFiscalItem.ValorIcms = notaFiscalItem.BaseIcms * notaFiscalItem.AliquotaIcms;
                }
                notaFiscalItem.NomeProduto = itemPedido.NomeProduto;
                notaFiscalItem.CodigoProduto = itemPedido.CodigoProduto;

                notaFiscalItem.BaseIpi = itemPedido.ValorItemPedido;
                // alterado conforme abaixo do comentário, usando ternário
                //if (itemPedido.Brinde)
                //{
                //    notaFiscalItem.AliquotaIpi = 0;
                //}
                //else
                //{
                //    notaFiscalItem.AliquotaIpi = 0.1;
                //}
                notaFiscalItem.AliquotaIpi = (itemPedido.Brinde) ? 0 : 0.1;
                notaFiscalItem.ValorIpi = notaFiscalItem.BaseIpi * notaFiscalItem.AliquotaIpi;

                notaFiscalItem.PercDesconto = RetornarPercDesconto(notaFiscal);

                notaFiscal.ItensDaNotaFiscal.Add(notaFiscalItem);
            }

            #region [Salvar a NF no arquivo Xml]
            if (!SerializarNotaFiscal(notaFiscal))
            {
                throw new Exception("Não foi possível salvar a Nota Fiscal em arquivo");
            }
            #endregion

            #region [Salvar a NF no BD]
            NotaFiscalRepository notaFiscalRepository = new NotaFiscalRepository();
            notaFiscalRepository.SalvarNotaFiscal(notaFiscal.Id, notaFiscal.NumeroNotaFiscal, notaFiscal.Serie, notaFiscal.NomeCliente, notaFiscal.EstadoDestino, notaFiscal.EstadoOrigem, notaFiscal.ItensDaNotaFiscal);

            if (notaFiscalRepository.ResultType == ConexaoResultType.DbError)
            {
                throw new Exception(notaFiscalRepository.ResultMessage);
            }
            #endregion
        }

        private bool SerializarNotaFiscal(NotaFiscal notaFiscal)
        {
            try
            {
                // verificar se o diretório padrão existe, senão criá-lo
                string pathXml = Directory.GetCurrentDirectory() + @"\xml";
                if (!Directory.Exists(pathXml))
                {
                    Directory.CreateDirectory(pathXml);
                }

                // verificar se o arquivo padrão existe, pois precisa ser apagado antes de gerar um novo
                string fileXml = pathXml + @"\NotaFiscal.xml";
                if (File.Exists(fileXml))
                {
                    File.Delete(fileXml);
                }

                // salvar a classe NotaFiscal em um arquivo Xml
                XmlSerializer serializer = new XmlSerializer(typeof(NotaFiscal));
                FileStream stream = new FileStream(fileXml, FileMode.OpenOrCreate);
                serializer.Serialize(stream, notaFiscal);
                stream.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private string RetornarCfop(NotaFiscal notaFiscal)
        {
            string retorno = string.Empty;

            if ((notaFiscal.EstadoOrigem == "SP") || (notaFiscal.EstadoOrigem == "MG"))
            {
                switch (notaFiscal.EstadoDestino)
                {
                    case "RJ":
                        retorno = "6.000";
                        break;
                    case "PE":
                        retorno = "6.001";
                        break;
                    case "MG":
                        retorno = "6.002";
                        break;
                    case "PB":
                        retorno = "6.003";
                        break;
                    case "PR":
                        retorno = "6.004";
                        break;
                    case "PI":
                        retorno = "6.005";
                        break;
                    case "RO":
                        retorno = "6.006";
                        break;
                    case "SE":
                        retorno = "6.007";
                        break;
                    case "TO":
                        retorno = "6.008";
                        break;
                    //case "SE":  // estava em duplicidade, precisa verificar qual a correta
                    //    retorno = "6.009";
                    //    break;
                    case "PA":
                        retorno = "6.010";
                        break;
                }
            }

            return retorno;


            //
            // código antigo (que estava na classe NotaFiscal), deixei apenas para ser analisado por quem for corrigir meu código!
            //

            //if ((this.EstadoOrigem == "SP") && (this.EstadoDestino == "RJ"))
            //{
            //    return "6.000";
            //}
            //else if ((this.EstadoOrigem == "SP") && (this.EstadoDestino == "PE"))
            //{
            //    return "6.001";
            //}
            //else if ((this.EstadoOrigem == "SP") && (this.EstadoDestino == "MG"))
            //{
            //    return "6.002";
            //}
            //else if ((this.EstadoOrigem == "SP") && (this.EstadoDestino == "PB"))
            //{
            //    return "6.003";
            //}
            //else if ((this.EstadoOrigem == "SP") && (this.EstadoDestino == "PR"))
            //{
            //    return "6.004";
            //}
            //else if ((this.EstadoOrigem == "SP") && (this.EstadoDestino == "PI"))
            //{
            //    return "6.005";
            //}
            //else if ((this.EstadoOrigem == "SP") && (this.EstadoDestino == "RO"))
            //{
            //    return "6.006";
            //}
            //else if ((this.EstadoOrigem == "SP") && (this.EstadoDestino == "SE"))
            //{
            //    return "6.007";
            //}
            //else if ((this.EstadoOrigem == "SP") && (this.EstadoDestino == "TO"))
            //{
            //    return "6.008";
            //}
            //else if ((this.EstadoOrigem == "SP") && (this.EstadoDestino == "SE"))
            //{
            //    return "6.009";
            //}
            //else if ((this.EstadoOrigem == "SP") && (this.EstadoDestino == "PA"))
            //{
            //    return "6.010";
            //}
            //else if ((this.EstadoOrigem == "MG") && (this.EstadoDestino == "RJ"))
            //{
            //    return "6.000";
            //}
            //else if ((this.EstadoOrigem == "MG") && (this.EstadoDestino == "PE"))
            //{
            //    return "6.001";
            //}
            //else if ((this.EstadoOrigem == "MG") && (this.EstadoDestino == "MG"))
            //{
            //    return "6.002";
            //}
            //else if ((this.EstadoOrigem == "MG") && (this.EstadoDestino == "PB"))
            //{
            //    return "6.003";
            //}
            //else if ((this.EstadoOrigem == "MG") && (this.EstadoDestino == "PR"))
            //{
            //    return "6.004";
            //}
            //else if ((this.EstadoOrigem == "MG") && (this.EstadoDestino == "PI"))
            //{
            //    return "6.005";
            //}
            //else if ((this.EstadoOrigem == "MG") && (this.EstadoDestino == "RO"))
            //{
            //    return "6.006";
            //}
            //else if ((this.EstadoOrigem == "MG") && (this.EstadoDestino == "SE"))
            //{
            //    return "6.007";
            //}
            //else if ((this.EstadoOrigem == "MG") && (this.EstadoDestino == "TO"))
            //{
            //    return "6.008";
            //}
            //else if ((this.EstadoOrigem == "MG") && (this.EstadoDestino == "SE"))
            //{
            //    return "6.009";
            //}
            //else if ((this.EstadoOrigem == "MG") && (this.EstadoDestino == "PA"))
            //{
            //    return "6.010";
            //}
        }

        private double RetornarPercDesconto(NotaFiscal notaFiscal)
        {
            double retorno = 0;

            switch (notaFiscal.EstadoDestino)
            {
                case "SP":
                case "MG":
                case "RJ":
                case "ES":
                    retorno = 0.1;
                    break;
                //default:
                //    retorno = 0;
                //    break;
            }

            return retorno;
        }
    }
}
