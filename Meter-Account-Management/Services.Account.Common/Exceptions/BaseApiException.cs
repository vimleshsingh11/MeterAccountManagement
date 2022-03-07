using System;
using System.Collections.ObjectModel;

namespace Services.Account.Common.Exceptions
{
    public class BaseApiException
    {
        public string Message { get; }
        public string Code { get; }
        #nullable enable
        public Collection<FieldError>? FieldErrors { get; }
        #nullable disable

        public BaseApiException(string message , string code, Collection<FieldError>? fieldErrors)
        {
            this.Message = message;
            this.Code = code;
            this.FieldErrors = fieldErrors;
        }
    }

    public class FieldError
    {

        public FieldError(string fieldName, string errorType)
        {
            this.FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
            this.ErrorType = errorType ?? throw new ArgumentNullException(nameof(errorType));
        }

        public string FieldName { get; set; }
        public string ErrorType { get; set; }
    }

}
