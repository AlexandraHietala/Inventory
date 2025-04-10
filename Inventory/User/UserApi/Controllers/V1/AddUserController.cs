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
    [ControllerName("AddUserControllerV1")]
    [Route("api/v{version:apiVersion}/user")]
    public class AddUserControllerV1 : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserControllerValidatorV1 _controllerValidator;
        private readonly IAddUserWorkflowV1 _addUserWorkflow;

        public AddUserControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddUserControllerV1>();
            _configuration = configuration;
            _controllerValidator = new UserControllerValidatorV1();
            _addUserWorkflow = new AddUserWorkflowV1(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        [Route("adduser")]
        public async Task<IActionResult> AddUserV1(string name, string salt, string hash, int? roleId, string lastmodifiedby)
        {
            _logger.LogDebug("AddUser request received.");

            try
            {
                // Validate
                User user = new User()
                {
                    Name = name,
                    PassSalt = salt,
                    PassHash = hash,
                    RoleId = roleId,
                    LastModifiedBy = lastmodifiedby,
                    LastModifiedDate = DateTime.Now,
                    CreatedBy = lastmodifiedby,
                    CreatedDate = DateTime.Now
                };

                var failures = _controllerValidator.ValidateAdd(user);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                int id = await _addUserWorkflow.AddUser(user);

                // Respond
                _logger.LogInformation("AddUser success response.");
                return Ok(id);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100100001] AddUser ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[100100002] AddUser InvalidOperationException: {ioe}.");
                return NotFound("[100100002] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[100100003] AddUser Exception: {e}.");
                return Problem("[100100003] " + e.Message);
            }
        }
    }
}
