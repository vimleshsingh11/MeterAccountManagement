using System;
using System.Collections.ObjectModel;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ModelError = Microsoft.AspNetCore.Mvc.ModelBinding.ModelError;
using ModelErrorCollection = Microsoft.AspNetCore.Mvc.ModelBinding.ModelErrorCollection;

namespace Services.Account.Common.Exceptions
{
    public class ExceptionPayload
    {

        public BaseApiException Exception { get; set; }
#nullable enable
        private Collection<FieldError>? fieldErrors = new Collection<FieldError>();
#nullable disable

        public ExceptionPayload(ActionContext actionContext)
        {
            foreach( string errorField in actionContext.ModelState.Keys)
            {
                foreach (ModelError erroMessage in actionContext.ModelState[errorField].Errors)
                {
                    fieldErrors?.Add(new FieldError(errorField, erroMessage.ErrorMessage));
                }
            }
        }
        public ExceptionPayload(string message, string code)
        {
            Exception = new BaseApiException(message, code, null);
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}