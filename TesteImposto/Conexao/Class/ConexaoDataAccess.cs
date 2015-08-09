using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Conexao.Interface;
using Conexao.Enum;

namespace Conexao.Class
{
    public class ConexaoDataAccess : IConexaoDataAccess
    {
        private Boolean success = false;
        public Boolean Success
        {
            get { return this.success; }
            set { this.success = value; }
        }

        private ConexaoResultType resultType = ConexaoResultType.None;
        public ConexaoResultType ResultType
        {
            get { return this.resultType; }
            set { this.resultType = value; }
        }

        private String resultMessage = String.Empty;
        public String ResultMessage
        {
            get { return this.resultMessage; }
            set { this.resultMessage = value; }
        }

        protected void SetResult(ConexaoDataAccess conexaoDataAccess)
        {
            this.Success = conexaoDataAccess.Success;
            this.ResultType = conexaoDataAccess.ResultType;
            this.ResultMessage = conexaoDataAccess.ResultMessage;
        }
    }
}
