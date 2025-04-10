using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UserApi.Data.DataOperations.V1;
using UserApi.Data.Validators.V1;
using UserApi.Workflows.Validators.V1;

namespace UserApi.Workflows.Workflows.V1
{
    public interface IRemoveUserWorkflowV1
    {
        Task RemoveUser(int id, string lastmodifiedby);
    }

    public class RemoveUserWorkflowV1 : IRemoveUserWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IRemoveUserOperationsV1 _removeUserOperations;
        private readonly IUserDataValidatorV1 _userDataValidator;
        private readonly IRoleDataValidatorV1 _roleDataValidator;
        private readonly IUserWorkflowValidatorV1 _workflowValidator;

        public RemoveUserWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveUserWorkflowV1>();
            _configuration = configuration;
            _removeUserOperations = new RemoveUserOperationsV1(loggerFactory, configuration);
            _userDataValidator = new UserDataValidatorV1(loggerFactory, configuration);
            _roleDataValidator = new RoleDataValidatorV1(loggerFactory, configuration);
            _workflowValidator = new UserWorkflowValidatorV1(loggerFactory, configuration, _userDataValidator, _roleDataValidator);
        }

        public async Task RemoveUser(int id, string lastmodifiedby)
        {
            _logger.LogDebug("RemoveUser request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateUserId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeUserOperations.RemoveUser(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveUser success response.");
                return;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100300013] RemoveUser ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[100300014] RemoveUser Exception: {e}.");
                throw;
            }
        }
    }
}