using System.Net;
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
                context.Result = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Content = context.Exception.Message,
                    ContentType = "text/plain",
                };
                context.Exception = null;
            }
        }
    }
}
