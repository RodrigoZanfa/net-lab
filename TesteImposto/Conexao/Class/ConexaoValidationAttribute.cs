using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Conexao.Enum;

namespace Conexao.Class
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ConexaoValidationAttribute : Attribute
    {
        public ConexaoValidationType ValidationType { get; set; }

        public String Description { get; set; }

        public ConexaoValidationAttribute()
        {
            this.ValidationType = ConexaoValidationType.None;
        }

        public Boolean ValidateFlag(ConexaoValidationType validationType)
        {
            return ((ValidationType & validationType) == validationType);
        }
    }
}
