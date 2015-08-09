using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Conexao.Enum
{
    [Flags]  // Utilizar para os códigos sempre o múltiplo do anterior
    public enum ConexaoValidationType
    {
        None = 0,
        Required = 1,
        Integer = 2,
        Long = 4,
        Decimal = 8,
        Date = 16,
        DateTime = 32
    }
}
