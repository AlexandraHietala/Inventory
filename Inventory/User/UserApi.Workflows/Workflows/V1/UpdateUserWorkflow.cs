using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UserApi.Workflows.Validators.V1;
using UserApi.Data.DataOperations.V1;
using UserApi.Models.Classes.V1;
using UserApi.Models.Converters.V1;
using UserApi.Models.DTOs.V1;
using UserApi.Data.Validators.V1;

namespace UserApi.Workflows.Workflows.V1
{
    public interface IUpdateUserWorkflowV1
    {
        Task UpdateUser(User user);
    }

    public class UpdateUserWorkflowV1 : IUpdateUserWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUpdateUserOperationsV1 _updateUserOperations;
        private readonly IUserDataValidatorV1 _userDataValidator;
        private readonly IRoleDataValidatorV1 _roleDataValidator;
        private readonly IUserWorkflowValidatorV1 _workflowValidator;

        public UpdateUserWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateUserWorkflowV1>();
            _configuration = configuration;
            _updateUserOperations = new UpdateUserOperationsV1(loggerFactory, configuration);
            _userDataValidator = new UserDataValidatorV1(loggerFactory, configuration);
            _roleDataValidator = new RoleDataValidatorV1(loggerFactory, configuration);
            _workflowValidator = new UserWorkflowValidatorV1(loggerFactory, configuration, _userDataValidator, _roleDataValidator);
        }

        public async Task UpdateUser(User user)
        {
            _logger.LogDebug("UpdateUser request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateUpdate(user);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                UserDto userDto = UserConverter.ConvertUserToUserDto(user);
                await _updateUserOperations.UpdateUser(userDto);

                // Respond
                _logger.LogInformation("UpdateUser success response.");
                return;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100300015] UpdateUser ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[100300016] UpdateUser Exception: {e}.");
                throw;
            }
        }
    }
}