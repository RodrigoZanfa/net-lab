using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Conexao.Enum
{
    public enum ConexaoResultType
    {
        None,                  // Estado inicial

        //ConnectionSuccessful,  // Conexão com o banco de dados foi bem-sucedida

        RowsAffected,          // Um ou mais registros foram afetados pelo método ExecuteNonQuery()
        NoRowsAffected,        // Nenhum registro foi afetado pelo método ExecuteNonQuery()

        DataFound,             // Dados foram encontrados por meio dos métodos ExecuteReader(), ExecuteScalar() ou GetDataTable()
        NoDataFound,           // Dados não foram encontrados por meio dos métodos ExecuteReader(), ExecuteScalar() ou GetDataTable()

        DbError,               // Erro durante a execução de comando no banco de dados

        InvalidData            // Dados inválidos (usado em validação do BO e outras)
    }
}
