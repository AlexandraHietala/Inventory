using UserApi.Models.Classes.V1;
using UserApi.Models.System;

namespace UserApi.Validators.V1
{
    public interface IUserControllerValidatorV1
    {
        string ValidateAdd(User user);
        string ValidateUpdate(User user);
        string ValidateUserId(int id);
        string ValidateRoleId(int id);
    }

    public class UserControllerValidatorV1 : IUserControllerValidatorV1
    { 
        public string ValidateAdd(User user)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (user == null)
                failureList.Add(new ValidationFailure() { Code = 100200001, Message = "User object is invalid." });

            if (user != null && string.IsNullOrEmpty(user.Name))
                failureList.Add(new ValidationFailure() { Code = 100200002, Message = "Name is required." });

            if (user != null && string.IsNullOrEmpty(user.PassSalt))
                failureList.Add(new ValidationFailure() { Code = 100200003, Message = "Salt is required." });

            if (user != null && string.IsNullOrEmpty(user.PassHash))
                failureList.Add(new ValidationFailure() { Code = 100200004, Message = "Hash is required." });

            if (user != null && user.RoleId != null && user.RoleId == 0)
                failureList.Add(new ValidationFailure() { Code = 100200005, Message = "Role is invalid." });

            if (user != null && user.RoleId != null && user.RoleId < 1000)
                failureList.Add(new ValidationFailure() { Code = 100200006, Message = "Role is invalid." });

            if (user != null && user.RoleId != null && !int.TryParse(user.RoleId.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 100200007, Message = "Role is invalid." });

            if (user != null && user.RoleId != null && user.RoleId > 9999)
                failureList.Add(new ValidationFailure() { Code = 100200008, Message = "Role is invalid." });

            if (user != null && user.Name != null && user.Name.Length > 250)
                failureList.Add(new ValidationFailure() { Code = 100200009, Message = "Name is too long." });

            if (user != null && user.PassSalt != null && user.PassSalt.Length > 250)
                failureList.Add(new ValidationFailure() { Code = 100200010, Message = "Salt is too long." });

            if (user != null && user.PassHash != null && user.PassHash.Length > 250)
                failureList.Add(new ValidationFailure() { Code = 100200011, Message = "Hash is too long." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public string ValidateUpdate(User user)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (user == null)
                failureList.Add(new ValidationFailure() { Code = 100200012, Message = "User object is invalid." });

            if (user != null && user.Id == 0)
                failureList.Add(new ValidationFailure() { Code = 100200013, Message = "User Id is invalid." });

            if (user != null && user.Id < 0)
                failureList.Add(new ValidationFailure() { Code = 100200014, Message = "User Id is invalid." });

            if (user != null && user.Id > 99999)
                failureList.Add(new ValidationFailure() { Code = 100200015, Message = "User Id is invalid." });

            if (user != null && !int.TryParse(user.Id.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 100200016, Message = "User Id is invalid." });

            if (user != null && string.IsNullOrEmpty(user.Name))
                failureList.Add(new ValidationFailure() { Code = 100200017, Message = "Name is required." });

            if (user != null && string.IsNullOrEmpty(user.PassSalt))
                failureList.Add(new ValidationFailure() { Code = 100200018, Message = "Salt is required." });

            if (user != null && string.IsNullOrEmpty(user.PassHash))
                failureList.Add(new ValidationFailure() { Code = 100200019, Message = "Hash is required." });

            if (user != null && user.RoleId != null && user.RoleId == 0)
                failureList.Add(new ValidationFailure() { Code = 100200020, Message = "Role is invalid." });

            if (user != null && user.RoleId != null && user.RoleId < 1000)
                failureList.Add(new ValidationFailure() { Code = 100200021, Message = "Role is invalid." });

            if (user != null && user.RoleId != null && !int.TryParse(user.RoleId.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 100200022, Message = "Role is invalid." });

            if (user != null && user.RoleId != null && user.RoleId > 9999)
                failureList.Add(new ValidationFailure() { Code = 100200023, Message = "Role is invalid." });

            if (user != null && user.Name != null && user.Name.Length > 250)
                failureList.Add(new ValidationFailure() { Code = 100200024, Message = "Name is too long." });

            if (user != null && user.PassSalt != null && user.PassSalt.Length > 250)
                failureList.Add(new ValidationFailure() { Code = 100200025, Message = "Salt is too long." });

            if (user != null && user.PassHash != null && user.PassHash.Length > 250)
                failureList.Add(new ValidationFailure() { Code = 100200026, Message = "Hash is too long." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public string ValidateUserId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (id == 0)
                failureList.Add(new ValidationFailure() { Code = 100200027, Message = "User Id is invalid." });

            if (id < 0)
                failureList.Add(new ValidationFailure() { Code = 100200028, Message = "User Id is invalid." });

            if (id > 99999)
                failureList.Add(new ValidationFailure() { Code = 100200029, Message = "User Id is invalid." });


            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public string ValidateRoleId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (id == 0)
                failureList.Add(new ValidationFailure() { Code = 100200030, Message = "Role Id is invalid." });

            if (id < 0)
                failureList.Add(new ValidationFailure() { Code = 100200031, Message = "Role Id is invalid." });

            if (id > 99999)
                failureList.Add(new ValidationFailure() { Code = 100200032, Message = "Role Id is invalid." });


            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }
    }
}
