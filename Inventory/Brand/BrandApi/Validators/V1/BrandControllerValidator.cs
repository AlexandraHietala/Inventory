using BrandApi.Models.Classes.V1;
using BrandApi.Models.System;
using Microsoft.SqlServer.Server;
using System.Drawing;

namespace BrandApi.Validators.V1
{
    public interface IBrandControllerValidatorV1
    {
        string ValidateBrandId(int id);
        string ValidateAddBrand(Brand brand);
        string ValidateUpdateBrand(Brand brand);
    }

    public class BrandControllerValidatorV1 : IBrandControllerValidatorV1
    {
        public string ValidateAddBrand(Brand brand)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (brand == null)
                failureList.Add(new ValidationFailure() { Code = 300200001, Message = "Brand object is invalid." });

            if (brand != null && string.IsNullOrEmpty(brand.BrandName))
                failureList.Add(new ValidationFailure() { Code = 300200002, Message = "Name is required." });

            if (brand != null && brand.BrandName != null && brand.BrandName.Length > 50)
                failureList.Add(new ValidationFailure() { Code = 300200003, Message = "Name is too long." });

            if (brand != null && brand.Description != null && brand.Description.Length > 250)
                failureList.Add(new ValidationFailure() { Code = 300200004, Message = "Description is too long." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public string ValidateUpdateBrand(Brand brand)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (brand == null)
                failureList.Add(new ValidationFailure() { Code = 300200005, Message = "Brand object is invalid." });

            if (brand != null && brand.Id == 0)
                failureList.Add(new ValidationFailure() { Code = 300200006, Message = "Brand Id is invalid." });

            if (brand != null && brand.Id < 0)
                failureList.Add(new ValidationFailure() { Code = 300200007, Message = "Brand Id is invalid." });

            if (brand != null && brand.Id > 99999)
                failureList.Add(new ValidationFailure() { Code = 300200008, Message = "Brand Id is invalid." });

            if (brand != null && !int.TryParse(brand.Id.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 300200009, Message = "Brand Id is invalid." });

            if (brand != null && string.IsNullOrEmpty(brand.BrandName))
                failureList.Add(new ValidationFailure() { Code = 300200010, Message = "Name is required." });

            if (brand != null && brand.BrandName != null && brand.BrandName.Length > 50)
                failureList.Add(new ValidationFailure() { Code = 300200011, Message = "Name is too long." });

            if (brand != null && brand.Description != null && brand.Description.Length > 250)
                failureList.Add(new ValidationFailure() { Code = 300200012, Message = "Description is too long." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public string ValidateBrandId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (id == 0)
                failureList.Add(new ValidationFailure() { Code = 300200013, Message = "Brand Id is invalid." });

            if (id < 0)
                failureList.Add(new ValidationFailure() { Code = 300200014, Message = "Brand Id is invalid." });

            if (id > 99999)
                failureList.Add(new ValidationFailure() { Code = 300200015, Message = "Brand Id is invalid." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }
    }
}
