using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UserApi.Data.DataOperations.V1;
using UserApi.Models.Classes.V1;
using UserApi.Models.Converters.V1;
using UserApi.Models.DTOs.V1;
using UserApi.Workflows.Validators.V1;

namespace UserApi.Workflows.Workflows.V1
{
    public interface IAddUserWorkflowV1
    {
        Task<int> AddUser(User reqUser);
    }

    public class AddUserWorkflowV1 : IAddUserWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IAddUserOperationsV1 _addUserOperations;
        private readonly IVerifyOperationsV1 _verifyOperations;
        private readonly IWorkflowValidatorV1 _workflowValidator;


        public AddUserWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddUserWorkflowV1>();
            _configuration = configuration;
            _addUserOperations = new AddUserOperationsV1(loggerFactory, configuration);
            _verifyOperations = new VerifyOperationsV1(loggerFactory, configuration);
            _workflowValidator = new WorkflowValidatorV1(loggerFactory, configuration, _verifyOperations);
        }

        public async Task<int> AddUser(User user)
        {
            _logger.LogDebug("AddUser request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateAdd(user);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                UserDto userDto = UserConverter.ConvertUserToUserDto(user);
                int newUserId = await _addUserOperations.AddUser(userDto);

                // Respond
                _logger.LogInformation("AddUser success response.");
                return newUserId;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100300001] AddUser ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[100300002] AddUser Exception: {e}.");
                throw;
            }
        }
    }
}