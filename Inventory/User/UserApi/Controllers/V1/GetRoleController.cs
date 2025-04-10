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
    [ControllerName("GetRoleControllerV1")]
    [Route("api/v{version:apiVersion}/role")]
    public class GetRoleControllerV1 : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserControllerValidatorV1 _controllerValidator;
        private readonly IGetRoleWorkflowV1 _getRoleWorkflow;

        public GetRoleControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetRoleControllerV1>();
            _configuration = configuration;
            _controllerValidator = new UserControllerValidatorV1();
            _getRoleWorkflow = new GetRoleWorkflowV1(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getrole")]
        public async Task<IActionResult> GetRoleV1(int id)
        {
            _logger.LogDebug("GetRole request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateRoleId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                Role requestedRole = await _getRoleWorkflow.GetRole(id);

                // Respond
                _logger.LogInformation("GetRole success response.");
                return Ok(requestedRole);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100100007] GetRole ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[100100008] GetRole InvalidOperationException: {ioe}.");
                return NotFound("[100100008] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[100100009] GetRole Exception: {e}.");
                return Problem("[100100009] " + e.Message);
            }
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getroles")]
        public async Task<IActionResult> GetRolesV1()
        {
            _logger.LogDebug("GetRoles request received.");

            try
            {
                // Validate
                // Nothing to Validate

                // Process
                List<Role> requestedRoles = await _getRoleWorkflow.GetRoles();

                // Respond
                _logger.LogInformation("GetRoles success response.");
                return Ok(requestedRoles);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100100010] GetRoles ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[100100011] GetRoles InvalidOperationException: {ioe}.");
                return NotFound("[100100011] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[100100012] GetRoles Exception: {e}.");
                return Problem("[100100012] " + e.Message);
            }
        }
    }
}
