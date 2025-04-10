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
    [ControllerName("UpdateUserControllerV1")]
    [Route("api/v{version:apiVersion}/user")]
    public class UpdateUserControllerV1 : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserControllerValidatorV1 _controllerValidator;
        private readonly IUpdateUserWorkflowV1 _updateUserWorkflow;

        public UpdateUserControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateUserControllerV1>();
            _configuration = configuration;
            _controllerValidator = new UserControllerValidatorV1();
            _updateUserWorkflow = new UpdateUserWorkflowV1(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpPut]
        [Route("updateuser")]
        public async Task<IActionResult> UpdateUserV1(int id, string name, string salt, string hash, int? roleId, string lastmodifiedby)
        {
            _logger.LogDebug("UpdateUser request received.");

            try
            {
                // Validate
                User user = new User()
                {
                    Id = id,
                    Name = name,
                    PassSalt = salt,
                    PassHash = hash,
                    RoleId = roleId,
                    LastModifiedBy = lastmodifiedby,
                    LastModifiedDate = DateTime.Now,
                    CreatedBy = lastmodifiedby,
                    CreatedDate = DateTime.Now
                };

                var failures = _controllerValidator.ValidateUpdate(user);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _updateUserWorkflow.UpdateUser(user);

                // Respond
                _logger.LogInformation("UpdateUser success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100100022] UpdateUser ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[100100023] UpdateUser InvalidOperationException: {ioe}.");
                return NotFound("[100100023] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[100100024] UpdateUser Exception: {e}.");
                return Problem("[100100024] " + e.Message);
            }
        }
    }
}
