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
    public interface IGetRoleWorkflowV1
    {
        Task<Role> GetRole(int id);
        Task<List<Role>> GetRoles();
    }

    public class GetRoleWorkflowV1 : IGetRoleWorkflowV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IRoleOperationsV1 _roleOperations;
        private readonly IUserDataValidatorV1 _userDataValidator;
        private readonly IRoleDataValidatorV1 _roleDataValidator;
        private readonly IUserWorkflowValidatorV1 _workflowValidator;

        public GetRoleWorkflowV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetRoleWorkflowV1>();
            _configuration = configuration;
            _roleOperations = new GetRoleOperationsV1(loggerFactory, configuration);
            _userDataValidator = new UserDataValidatorV1(loggerFactory, configuration);
            _roleDataValidator = new RoleDataValidatorV1(loggerFactory, configuration);
            _workflowValidator = new UserWorkflowValidatorV1(loggerFactory, configuration, _userDataValidator, _roleDataValidator);
        }

        public async Task<Role> GetRole(int id)
        {
            _logger.LogDebug("GetRole request received.");

            try
            {
                // Validate
                var failures = await _workflowValidator.ValidateRoleId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                RoleDto roleDto = await _roleOperations.GetRole(id);
                Role role = RoleConverter.ConvertRoleDtoToRole(roleDto);

                // Respond
                _logger.LogInformation("GetRole success response.");
                return role;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100300005] GetRole ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[100300006] GetRole Exception: {e}.");
                throw;
            }
        }

        public async Task<List<Role>> GetRoles()
        {
            _logger.LogDebug("GetRoles request received.");

            try
            {
                // Validate
                // Nothing to validate

                // Process
                List<RoleDto> roleDtos = await _roleOperations.GetRoles();
                List<Role> roles = RoleConverter.ConvertListRoleDtosToListRoles(roleDtos);

                // Respond
                _logger.LogInformation("GetRoles success response.");
                return roles;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100300007] GetRoles ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[100300008] GetRoles Exception: {e}.");
                throw;
            }
        }
    }
}