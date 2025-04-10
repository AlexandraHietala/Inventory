using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UserApi.Data.DataOperations.V1;
using UserApi.Data.Validators.V1;
using UserApi.Models.Classes.V1;
using UserApi.Models.System;

namespace UserApi.Workflows.Validators.V1
{
    public interface IUserWorkflowValidatorV1
    {
        Task<string> ValidateAdd(User user);
        Task<string> ValidateUpdate(User user);
        Task<string> ValidateUserId(int id);
        Task<string> ValidateRoleId(int id);
    }

    public class UserWorkflowValidatorV1 : IUserWorkflowValidatorV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserDataValidatorV1 _userDataValidator;
        private readonly IRoleDataValidatorV1 _roleDataValidator;

        public UserWorkflowValidatorV1(ILoggerFactory loggerFactory, IConfiguration configuration, IUserDataValidatorV1 userDataValidator, IRoleDataValidatorV1 roleDataValidator)
        {
            _logger = loggerFactory.CreateLogger<UserWorkflowValidatorV1>();
            _configuration = configuration;
            _userDataValidator = userDataValidator;
            _roleDataValidator = roleDataValidator;
        }

        public async Task<string> ValidateAdd(User user)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (user == null)
                failureList.Add(new ValidationFailure() { Code = 100400001, Message = "User object is invalid." });

            if (user != null && user.RoleId != null && user.RoleId.HasValue)
            {
                bool roleExists = await ValidateRoleExists(user.RoleId.Value);
                if (!roleExists) failureList.Add(new ValidationFailure() { Code = 100400002, Message = "Role does not exist." });
            }

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateUpdate(User user)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (user == null)
                failureList.Add(new ValidationFailure() { Code = 100400003, Message = "User object is invalid." });

            if (user != null)
            {
                bool userExists = await ValidateUserExists(user.Id);
                if (!userExists) failureList.Add(new ValidationFailure() { Code = 100400004, Message = "User does not exist." });
            }

            if (user != null && user.RoleId != null && user.RoleId.HasValue)
            {
                bool roleExists = await ValidateRoleExists(user.RoleId.Value);
                if (!roleExists) failureList.Add(new ValidationFailure() { Code = 100400005, Message = "Role does not exist." });
            }

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateUserId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            bool userExists = await ValidateUserExists(id);
            if (!userExists) failureList.Add(new ValidationFailure() { Code = 100400006, Message = "User does not exist." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateRoleId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            bool roleExists = await ValidateRoleExists(id);
            if (!roleExists) failureList.Add(new ValidationFailure() { Code = 100400007, Message = "Role does not exist." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        private async Task<bool> ValidateRoleExists(int id)
        {
            bool valid = await _roleDataValidator.VerifyRole(id);
            return valid;
        }

        private async Task<bool> ValidateUserExists(int id)
        {
            bool valid = await _userDataValidator.VerifyUser(id);
            return valid;
        }


    }
}
