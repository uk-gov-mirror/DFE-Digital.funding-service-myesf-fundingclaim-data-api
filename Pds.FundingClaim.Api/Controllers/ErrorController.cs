using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pds.Core.Logging;

namespace Pds.FundingClaim.Api.Controllers
{
    /// <summary>
    /// Controller for handling and logging errors.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly ILoggerAdapter<ErrorController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ErrorController(ILoggerAdapter<ErrorController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Log the exception and return a problem details response.
        /// </summary>
        /// <remarks>This is called in production so do not add include details of the error in the response.</remarks>
        /// <returns>The problem details object.</returns>
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [HttpGet]
        public ActionResult<ProblemDetails> Error()
        {
            var errorContext = HttpContext.Features.Get<IExceptionHandlerFeature>();

            _logger.LogError(errorContext.Error, "An error occurred in the Funding Claims API: {ErrorMessage}", errorContext.Error.Message);

            return Problem();
        }
    }
}