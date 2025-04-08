using ItemApi.Models.Classes.V1;
using ItemApi.Models.System;
using Microsoft.SqlServer.Server;
using System.Drawing;

namespace ItemApi.Validators.V1
{
    public interface IItemCommentControllerValidatorV1
    {
        string ValidateAddItemComment(ItemComment comment);
        string ValidateUpdateItemComment(ItemComment comment);
    }

    public class ItemCommentControllerValidatorV1 : IItemCommentControllerValidatorV1
    {
        public string ValidateAddItemComment(ItemComment comment)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (comment == null)
                failureList.Add(new ValidationFailure() { Code = 200200017, Message = "Comment object is invalid." });

            if (comment != null && comment.ItemId == 0)
                failureList.Add(new ValidationFailure() { Code = 200200018, Message = "Item Id is invalid." });

            if (comment != null && comment.ItemId < 0)
                failureList.Add(new ValidationFailure() { Code = 200200019, Message = "Item Id is invalid." });

            if (comment != null && comment.ItemId > 99999)
                failureList.Add(new ValidationFailure() { Code = 200200020, Message = "Item Id is invalid." });

            if (comment != null && !int.TryParse(comment.ItemId.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 200200021, Message = "Item Id is invalid." });

            if (comment != null && string.IsNullOrEmpty(comment.Comment))
                failureList.Add(new ValidationFailure() { Code = 200200022, Message = "Comment is required." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public string ValidateUpdateItemComment(ItemComment comment)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (comment == null)
                failureList.Add(new ValidationFailure() { Code = 200200023, Message = "Comment object is invalid." });

            if (comment != null && comment.Id == 0)
                failureList.Add(new ValidationFailure() { Code = 200200024, Message = "Comment Id is invalid." });

            if (comment != null && comment.Id < 0)
                failureList.Add(new ValidationFailure() { Code = 200200025, Message = "Comment Id is invalid." });

            if (comment != null && comment.Id > 99999)
                failureList.Add(new ValidationFailure() { Code = 200200026, Message = "Comment Id is invalid." });

            if (comment != null && !int.TryParse(comment.Id.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 200200027, Message = "Comment Id is invalid." });

            if (comment != null && comment.ItemId == 0)
                failureList.Add(new ValidationFailure() { Code = 200200028, Message = "Item Id is invalid." });

            if (comment != null && comment.ItemId < 0)
                failureList.Add(new ValidationFailure() { Code = 200200029, Message = "Item Id is invalid." });

            if (comment != null && comment.ItemId > 99999)
                failureList.Add(new ValidationFailure() { Code = 200200030, Message = "Item Id is invalid." });

            if (comment != null && !int.TryParse(comment.ItemId.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 200200031, Message = "Item Id is invalid." });

            if (comment != null && string.IsNullOrEmpty(comment.Comment))
                failureList.Add(new ValidationFailure() { Code = 200200032, Message = "Comment is required." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }
    }
}
