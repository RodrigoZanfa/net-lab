using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Conexao.Enum;

namespace Conexao.Class
{
    public class ConexaoBO
    {
        protected List<String> errorMessageList;

        //public ConexaoBO()
        //{
        //    this.errorMessageList = new List<String>();
        //}

        //public List<String> GetErrorList()
        //{
        //    return this.errorMessageList;
        //}

        public String GetHtmlErrorMessage()
        {
            StringBuilder htmlErrorMessage = new StringBuilder();

            String skipLine = String.Empty;

            foreach (String value in this.errorMessageList)
            {
                htmlErrorMessage.Append(skipLine + value);

                skipLine = "<br />";
            }

            return htmlErrorMessage.ToString();
        }

        public virtual Boolean ValidateProperties()
        {
            this.errorMessageList = new List<String>();

            Object instanceBO = this;

            Type classType = instanceBO.GetType();

            PropertyInfo[] propertyList = classType.GetProperties();

            foreach (PropertyInfo classProperty in propertyList)
            {
                Attribute[] attributeList = Attribute.GetCustomAttributes(classProperty);

                ConexaoValidationAttribute validationProperty = null;

                foreach (Attribute propertyAttribute in attributeList)
                {
                    if (propertyAttribute is ConexaoValidationAttribute)
                    {
                        validationProperty = (ConexaoValidationAttribute)propertyAttribute;

                        break;
                    }
                }

                if (validationProperty != null)
                {
                    Boolean validProperty = true;

                    Object value = classProperty.GetValue(instanceBO, null);

                    String fieldDescription = validationProperty.Description;

                    if (validationProperty.ValidateFlag(ConexaoValidationType.Required))
                    {
                        Boolean empty = ((value == null) || (value.ToString().Trim().Length == 0));

                        validProperty = !empty;

                        if (empty)
                        {
                            this.errorMessageList.Add(fieldDescription + " deve ser preenchido.");
                        }
                    }

                    // Validação de preenchimento requerido deve ser true
                    // Valor da propriedade deve ser diferente de nulo e não pode ser vazio (ou com espaços em branco)
                    if ((validProperty) && (value != null) && (value.ToString().Trim().Length > 0))
                    {
                        if (validationProperty.ValidateFlag(ConexaoValidationType.Integer))
                        {
                            Int32 intValue;

                            if (Int32.TryParse(value.ToString(), out intValue))
                            {
                                classProperty.SetValue(instanceBO, intValue, null);
                            }
                            else
                            {
                                this.errorMessageList.Add(fieldDescription + " deve ser um número inteiro.");
                            }
                        }
                        else if (validationProperty.ValidateFlag(ConexaoValidationType.Long))
                        {
                            Int64 longValue;

                            if (Int64.TryParse(value.ToString(), out longValue))
                            {
                                classProperty.SetValue(instanceBO, longValue, null);
                            }
                            else
                            {
                                this.errorMessageList.Add(fieldDescription + " deve ser um número inteiro.");
                            }
                        }
                        else if (validationProperty.ValidateFlag(ConexaoValidationType.Decimal))
                        {
                            Decimal decimalValue;

                            if (Decimal.TryParse(value.ToString(), out decimalValue))
                            {
                                classProperty.SetValue(instanceBO, decimalValue, null);
                            }
                            else
                            {
                                this.errorMessageList.Add(fieldDescription + " deve ser um número decimal.");
                            }
                        }
                        else if ((validationProperty.ValidateFlag(ConexaoValidationType.Date)) || (validationProperty.ValidateFlag(ConexaoValidationType.DateTime)))
                        {
                            DateTime dateTimeValue;

                            if (DateTime.TryParse(value.ToString(), out dateTimeValue))
                            {
                                classProperty.SetValue(instanceBO, dateTimeValue, null);
                            }
                            else
                            {
                                this.errorMessageList.Add(fieldDescription + " deve ser uma data válida.");
                            }
                        }
                    }
                }
            }

            // Será considerado válido (true) caso não hajam mensagens de erro
            return (this.errorMessageList.Count == 0);
        }
    }
}
