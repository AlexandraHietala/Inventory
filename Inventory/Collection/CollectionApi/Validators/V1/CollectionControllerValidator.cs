using CollectionApi.Models.Classes.V1;
using CollectionApi.Models.System;
using Microsoft.SqlServer.Server;
using System.Drawing;

namespace CollectionApi.Validators.V1
{
    public interface ICollectionControllerValidatorV1
    {
        string ValidateCollectionId(int id);
        string ValidateAddCollection(Collection item);
        string ValidateUpdateCollection(Collection item);
    }

    public class CollectionControllerValidatorV1 : ICollectionControllerValidatorV1
    {
        public string ValidateAddCollection(Collection collection)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (collection == null)
                failureList.Add(new ValidationFailure() { Code = 500200001, Message = "Collection object is invalid." });

            if (collection != null && string.IsNullOrEmpty(collection.CollectionName))
                failureList.Add(new ValidationFailure() { Code = 500200002, Message = "Name is required." });

            if (collection != null && collection.CollectionName != null && collection.CollectionName.Length > 50)
                failureList.Add(new ValidationFailure() { Code = 500200003, Message = "Name is too long." });

            if (collection != null && collection.Description != null && collection.Description.Length > 250)
                failureList.Add(new ValidationFailure() { Code = 500200004, Message = "Description is too long." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public string ValidateUpdateCollection(Collection collection)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (collection == null)
                failureList.Add(new ValidationFailure() { Code = 500200005, Message = "Collection object is invalid." });

            if (collection != null && collection.Id == 0)
                failureList.Add(new ValidationFailure() { Code = 500200006, Message = "Collection Id is invalid." });

            if (collection != null && collection.Id < 0)
                failureList.Add(new ValidationFailure() { Code = 500200007, Message = "Collection Id is invalid." });

            if (collection != null && collection.Id > 99999)
                failureList.Add(new ValidationFailure() { Code = 500200008, Message = "Collection Id is invalid." });

            if (collection != null && !int.TryParse(collection.Id.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 500200009, Message = "Collection Id is invalid." });

            if (collection != null && string.IsNullOrEmpty(collection.CollectionName))
                failureList.Add(new ValidationFailure() { Code = 500200010, Message = "Name is required." });

            if (collection != null && collection.CollectionName != null && collection.CollectionName.Length > 50)
                failureList.Add(new ValidationFailure() { Code = 500200011, Message = "Name is too long." });

            if (collection != null && collection.Description != null && collection.Description.Length > 250)
                failureList.Add(new ValidationFailure() { Code = 500200012, Message = "Description is too long." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public string ValidateCollectionId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (id == 0)
                failureList.Add(new ValidationFailure() { Code = 500200013, Message = "Collection Id is invalid." });

            if (id < 0)
                failureList.Add(new ValidationFailure() { Code = 500200014, Message = "Collection Id is invalid." });

            if (id > 99999)
                failureList.Add(new ValidationFailure() { Code = 500200015, Message = "Collection Id is invalid." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }
    }
}
