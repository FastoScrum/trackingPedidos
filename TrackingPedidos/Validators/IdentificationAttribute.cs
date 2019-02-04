using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrackingPedidos.Validators
{
    public class IdentificationAttribute : ValidationAttribute, IClientModelValidator
    {
        public IdentificationAttribute()
        {
            this.ErrorMessage = "Cédula inválida.";
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-identification", this.ErrorMessage);
        }

        private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }

            attributes.Add(key, value);
            return true;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                string identification = (string)value;
                return this.ValidarCedula(identification)
                     ? ValidationResult.Success : new ValidationResult("Cédula inválida.");
            }
            catch
            {
                return new ValidationResult(this.ErrorMessage);
            }
        }

        public override bool IsValid(object value)
        {
            string identification = (string)value;
            return this.ValidarCedula(identification);
        }

        private bool ValidarCedula(string identificacion)
        {
            if (identificacion == null)
            {
                return false;
            }
            if (identificacion.Length < 10)
            {
                return false;
            }
            int pares = 0, impares = 0;
            for (int i = 0; i < 9; i++)
            {
                int digito = int.Parse(identificacion[i].ToString());

                if (i % 2 == 0)
                {
                    var multi = digito * 2;
                    if (multi < 10)
                    {
                        pares += multi;
                    }
                    else
                    {
                        pares += multi - 9;
                    }
                }
                else
                {
                    impares += digito;
                }
            }
            int suma = pares + impares;
            int digitoVerificador = -1;
            for (int j = 10; j <= 60; j = j + 10)
            {
                if (suma <= j)
                {
                    digitoVerificador = j - suma;
                    break;
                }
            }
            return int.Parse(identificacion[9].ToString()) == digitoVerificador;
        }
    }
}