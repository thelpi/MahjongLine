using System.Net;
using MahjongLineServer.Controllers.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MahjongLineServer.Controllers.Filters
{
    /// <summary>
    /// Exception filter.
    /// </summary>
    /// <seealso cref="IExceptionFilter"/>
    public class ExceptionFilter : IExceptionFilter
    {
        /// <inheritdoc />
        public void OnException(ExceptionContext context)
        {
            if (context?.Exception != null)
            {
                var statusCode = HttpStatusCode.InternalServerError;
                if (context.Exception.GetType() == typeof(InvalidGameIdentifierException)
                    || context.Exception.GetType() == typeof(InvalidPlayerIndexException))
                {
                    statusCode = HttpStatusCode.BadRequest;
                }

                context.Result = new ContentResult
                {
                    StatusCode = (int)statusCode,
                    Content = context.Exception.Message,
                    ContentType = "text/plain",
                };
                context.Exception = null;
            }
        }
    }
}
