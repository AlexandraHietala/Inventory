using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UserApi.Models;
using UserApi.Validators.V1;
using UserApi.Workflows.Workflows.V1;

namespace UserApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("RemoveUserControllerV1")]
    [Route("api/v{version:apiVersion}/user")]
    public class RemoveUserControllerV1 : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserControllerValidatorV1 _controllerValidator;
        private readonly IRemoveUserWorkflowV1 _removeUserWorkflow;

        public RemoveUserControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveUserControllerV1>();
            _configuration = configuration;
            _controllerValidator = new UserControllerValidatorV1();
            _removeUserWorkflow = new RemoveUserWorkflowV1(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpDelete]
        [Route("removeuser")]
        public async Task<IActionResult> RemoveUserV1(int id, string lastmodifiedby)
        {
            _logger.LogDebug("RemoveUser request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateUserId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeUserWorkflow.RemoveUser(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveUser success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100100019] RemoveUser ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[100100020] RemoveUser InvalidOperationException: {ioe}.");
                return NotFound("[100100020] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[100100021] RemoveUser Exception: {e}.");
                return Problem("[100100021] " + e.Message);
            }
        }
    }
}
