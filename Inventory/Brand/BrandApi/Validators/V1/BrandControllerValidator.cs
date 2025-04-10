using BrandApi.Models.Classes.V1;
using BrandApi.Models.System;
using Microsoft.SqlServer.Server;
using System.Drawing;

namespace BrandApi.Validators.V1
{
    public interface IBrandControllerValidatorV1
    {
        string ValidateAddBrand(Brand brand);
        string ValidateUpdateBrand(Brand brand);
    }

    public class BrandControllerValidatorV1 : IBrandControllerValidatorV1
    {
        public string ValidateAddBrand(Brand brand)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (brand == null)
                failureList.Add(new ValidationFailure() { Code = 200200001, Message = "Brand object is invalid." });

            if (brand != null && string.IsNullOrEmpty(brand.BrandName))
                failureList.Add(new ValidationFailure() { Code = 200200002, Message = "Name is required." });

            if (brand != null && brand.BrandName != null && brand.BrandName.Length > 50)
                failureList.Add(new ValidationFailure() { Code = 200200003, Message = "Name is too long." });

            if (brand != null && brand.Description != null && brand.Description.Length > 250)
                failureList.Add(new ValidationFailure() { Code = 200200004, Message = "Description is too long." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public string ValidateUpdateBrand(Brand brand)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (brand == null)
                failureList.Add(new ValidationFailure() { Code = 200200005, Message = "Brand object is invalid." });

            if (brand != null && brand.Id == 0)
                failureList.Add(new ValidationFailure() { Code = 200200006, Message = "Brand Id is invalid." });

            if (brand != null && brand.Id < 0)
                failureList.Add(new ValidationFailure() { Code = 200200007, Message = "Brand Id is invalid." });

            if (brand != null && brand.Id > 99999)
                failureList.Add(new ValidationFailure() { Code = 200200008, Message = "Brand Id is invalid." });

            if (brand != null && !int.TryParse(brand.Id.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 200200009, Message = "Brand Id is invalid." });

            if (brand != null && string.IsNullOrEmpty(brand.BrandName))
                failureList.Add(new ValidationFailure() { Code = 200200011, Message = "Name is required." });

            if (brand != null && brand.BrandName != null && brand.BrandName.Length > 50)
                failureList.Add(new ValidationFailure() { Code = 200200012, Message = "Name is too long." });

            if (brand != null && brand.Description != null && brand.Description.Length > 250)
                failureList.Add(new ValidationFailure() { Code = 200200013, Message = "Description is too long." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }
    }
}
