using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Conexao.Enum;
using Conexao.Interface;

namespace Conexao.Class
{
    public class ConexaoServico : ConexaoDataAccess
    {
        protected Boolean ValidateBO(ConexaoBO conexaoBO)
        {
            Boolean valido = conexaoBO.ValidateProperties();

            if (valido == false)
            {
                this.Success = false;
                this.ResultType = ConexaoResultType.InvalidData;
                this.ResultMessage = conexaoBO.GetHtmlErrorMessage();
            }

            return valido;
        }
    }
}
