using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UserApi.Models.Classes.V1;
using UserApi.Validators.V1;
using UserApi.Workflows.Workflows.V1;

namespace UserApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("GetAuthControllerV1")]
    [Route("api/v{version:apiVersion}/auth")]
    public class GetAuthControllerV1 : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IControllerValidatorV1 _controllerValidator;
        private readonly IGetAuthWorkflowV1 _getAuthWorkflow;

        public GetAuthControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetAuthControllerV1>();
            _configuration = configuration;
            _controllerValidator = new ControllerValidatorV1();
            _getAuthWorkflow = new GetAuthWorkflowV1(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getauth")]
        public async Task<IActionResult> GetAuthV1(int id)
        {
            _logger.LogDebug("GetAuth request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                Auth auth = await _getAuthWorkflow.GetAuth(id);

                // Respond
                _logger.LogInformation("GetAuth success response.");
                return Ok(auth);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100100004] GetAuth ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[100100005] GetAuth InvalidOperationException: {ioe}.");
                return NotFound("[100100005] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[100100006] GetAuth Exception: {e}.");
                return Problem("[100100006] " + e.Message);
            }
        }

    }
}
