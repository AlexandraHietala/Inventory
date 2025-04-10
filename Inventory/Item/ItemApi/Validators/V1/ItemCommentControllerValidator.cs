using ItemApi.Models.Classes.V1;
using ItemApi.Models.System;
using Microsoft.SqlServer.Server;
using System.Drawing;

namespace ItemApi.Validators.V1
{
    public interface IItemCommentControllerValidatorV1
    {
        string ValidateItemCommentId(int id);
        string ValidateAddItemComment(ItemComment comment);
        string ValidateUpdateItemComment(ItemComment comment);
    }

    public class ItemCommentControllerValidatorV1 : IItemCommentControllerValidatorV1
    {
        public string ValidateAddItemComment(ItemComment comment)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (comment == null)
                failureList.Add(new ValidationFailure() { Code = 200200001, Message = "Comment object is invalid." });

            if (comment != null && comment.ItemId == 0)
                failureList.Add(new ValidationFailure() { Code = 200200002, Message = "Item Id is invalid." });

            if (comment != null && comment.ItemId < 0)
                failureList.Add(new ValidationFailure() { Code = 200200003, Message = "Item Id is invalid." });

            if (comment != null && comment.ItemId > 99999)
                failureList.Add(new ValidationFailure() { Code = 200200004, Message = "Item Id is invalid." });

            if (comment != null && !int.TryParse(comment.ItemId.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 200200005, Message = "Item Id is invalid." });

            if (comment != null && string.IsNullOrEmpty(comment.Comment))
                failureList.Add(new ValidationFailure() { Code = 200200006, Message = "Comment is required." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public string ValidateUpdateItemComment(ItemComment comment)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (comment == null)
                failureList.Add(new ValidationFailure() { Code = 200200007, Message = "Comment object is invalid." });

            if (comment != null && comment.Id == 0)
                failureList.Add(new ValidationFailure() { Code = 200200008, Message = "Comment Id is invalid." });

            if (comment != null && comment.Id < 0)
                failureList.Add(new ValidationFailure() { Code = 200200009, Message = "Comment Id is invalid." });

            if (comment != null && comment.Id > 99999)
                failureList.Add(new ValidationFailure() { Code = 200200010, Message = "Comment Id is invalid." });

            if (comment != null && !int.TryParse(comment.Id.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 200200011, Message = "Comment Id is invalid." });

            if (comment != null && comment.ItemId == 0)
                failureList.Add(new ValidationFailure() { Code = 200200012, Message = "Item Id is invalid." });

            if (comment != null && comment.ItemId < 0)
                failureList.Add(new ValidationFailure() { Code = 200200013, Message = "Item Id is invalid." });

            if (comment != null && comment.ItemId > 99999)
                failureList.Add(new ValidationFailure() { Code = 200200014, Message = "Item Id is invalid." });

            if (comment != null && !int.TryParse(comment.ItemId.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 200200015, Message = "Item Id is invalid." });

            if (comment != null && string.IsNullOrEmpty(comment.Comment))
                failureList.Add(new ValidationFailure() { Code = 200200016, Message = "Comment is required." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public string ValidateItemCommentId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (id == 0)
                failureList.Add(new ValidationFailure() { Code = 200200017, Message = "Id is invalid." });

            if (id < 0)
                failureList.Add(new ValidationFailure() { Code = 200200018, Message = "Id is invalid." });

            if (id > 99999)
                failureList.Add(new ValidationFailure() { Code = 200200019, Message = "Id is invalid." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }
    }
}
