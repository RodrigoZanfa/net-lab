using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Conexao.Enum;

namespace Conexao.Interface
{
    internal interface IConexaoDataAccess
    {
        Boolean Success { get; set; }
        ConexaoResultType ResultType { get; set; }
        String ResultMessage { get; set; }
    }
}
