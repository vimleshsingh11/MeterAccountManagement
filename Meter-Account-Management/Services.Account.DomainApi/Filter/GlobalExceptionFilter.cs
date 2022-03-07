using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Services.Account.DomainApi.Filter
{
    public class GlobalExceptionFilter: ExceptionFilterAttribute
    {
        private string defaultMsg = "An unexpected error occured while processing the request";
        private readonly ILogger _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// this method is invoked by the mvc framework for unhandled exceptions
        /// </summary>
        /// <param name="context">Exception context</param>
        public override void OnException(ExceptionContext context)
        {
            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)HttpStatusCode.InternalServerError;

            try
            {
                _logger.LogError(context.Exception, $"{context.Exception.GetType().Name}: {context.Exception.Message}");
                defaultMsg += $". , {context?.Exception?.Message}";
                _logger.LogInformation("Domain Global Exception " + context.Exception.Message);

                var innermostException = GetInnermostException(context.Exception);

                if (innermostException.GetType().Name == nameof(TimeoutException))
                    response.StatusCode = (int)HttpStatusCode.GatewayTimeout;
            }
            finally
            {
                context.ExceptionHandled = true;
                context.Result = new ObjectResult(new { Message = defaultMsg });
            }
        }

        private Exception GetInnermostException(Exception ex)
        {
            var currentException = ex;
            while (currentException.InnerException != null)
            {
                currentException = currentException.InnerException;
                _logger.LogError(currentException, $"{currentException.GetType().Name}: {currentException.Message}");
                _logger.LogInformation("Domain Global Exception inner exception" + currentException.Message);
            }

            return currentException;

        }
    }
}

