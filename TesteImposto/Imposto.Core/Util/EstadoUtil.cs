using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imposto.Core.Util
{
    public class EstadoUtil
    {
        public static bool ValidarUF(string uf)
        {
            string estados = "SP,MG,RJ,RS,SC,PR,ES,DF,MT,MS,GO,TO,BA,SE,AL,PB,PE,MA,RN,CE,PI,PA,AM,AP,FN,AC,RR,RO";
            if ((!string.IsNullOrEmpty(uf)) && (estados.Contains(uf)))
                return true;
            else
                return false;
        }

        public static string[] CarregarUF()
        {
            return new string[] { "", "SP", "MG", "RJ", "RS", "SC", "PR", "ES", "DF", "MT", "MS", "GO", "TO", "BA", "SE", "AL", "PB", "PE", "MA", "RN", "CE", "PI", "PA", "AM", "AP", "FN", "AC", "RR", "RO" };
        }
    }
}
