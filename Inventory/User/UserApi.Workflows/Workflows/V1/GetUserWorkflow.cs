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
    public interface IGetUserWorkflowV1
    {
        Task<User> GetUser(int id);
        Task<List<User>> GetUsers();
    }

    public class GetUserWorkflowV1 : IGetUserWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IGetUserOperationsV1 _getUserOperations;
        private readonly IUserDataValidatorV1 _userDataValidator;
        private readonly IRoleDataValidatorV1 _roleDataValidator;
        private readonly IUserWorkflowValidatorV1 _workflowValidator;

        public GetUserWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetUserWorkflowV1>();
            _configuration = configuration;
            _getUserOperations = new GetUserOperationsV1(loggerFactory, configuration);
            _userDataValidator = new UserDataValidatorV1(loggerFactory, configuration);
            _roleDataValidator = new RoleDataValidatorV1(loggerFactory, configuration);
            _workflowValidator = new UserWorkflowValidatorV1(loggerFactory, configuration, _userDataValidator, _roleDataValidator);
        }

        public async Task<User> GetUser(int id)
        {
            _logger.LogDebug("GetUser request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateUserId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                UserDto userDto = await _getUserOperations.GetUser(id);
                User user = UserConverter.ConvertUserDtoToUser(userDto);

                // Respond
                _logger.LogInformation("GetUser success response.");
                return user;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100300009] GetUser ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[100300010] GetUser Exception: {e}.");
                throw;
            }
        }

        public async Task<List<User>> GetUsers()
        {
            _logger.LogDebug("GetUsers request received.");

            try
            {
                // Validate
                // Nothing to validate

                // Process
                List<UserDto> userDtos = await _getUserOperations.GetUsers();
                List<User> users = UserConverter.ConvertListUserDtoToListUser(userDtos);

                // Respond
                _logger.LogInformation("GetUsers success response.");
                return users;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100300011] GetUsers ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[100300012] GetUsers Exception: {e}.");
                throw;
            }
        }
    }
}