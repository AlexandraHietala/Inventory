using ItemApi.Models.Classes.V1;
using ItemApi.Models.System;
using Microsoft.SqlServer.Server;
using System.Drawing;

namespace ItemApi.Validators.V1
{
    public interface IGeneralControllerValidatorV1
    {
        string ValidateId(int id);
    }

    public class GeneralControllerValidatorV1 : IGeneralControllerValidatorV1
    {
        public string ValidateId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (id == 0)
                failureList.Add(new ValidationFailure() { Code = 200200014, Message = "Id is invalid." });

            if (id < 0)
                failureList.Add(new ValidationFailure() { Code = 200200015, Message = "Id is invalid." });

            if (id > 99999)
                failureList.Add(new ValidationFailure() { Code = 200200016, Message = "Id is invalid." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }
    }
}
