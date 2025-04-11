using ItemApi.Models.Classes.V1;
using ItemApi.Models.System;
using Microsoft.SqlServer.Server;
using System.Drawing;

namespace ItemApi.Validators.V1
{
    public interface IItemControllerValidatorV1
    {
        string ValidateItemId(int id);
        string ValidateAddItem(Item item);
        string ValidateUpdateItem(Item item);
    }

    public class ItemControllerValidatorV1 : IItemControllerValidatorV1
    {
        public string ValidateAddItem(Item item)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (item == null)
                failureList.Add(new ValidationFailure() { Code = 200200020, Message = "Item object is invalid." });

            if (item != null && string.IsNullOrEmpty(item.Status))
                failureList.Add(new ValidationFailure() { Code = 200200022, Message = "Status is required." });

            if (item != null && string.IsNullOrEmpty(item.Type))
                failureList.Add(new ValidationFailure() { Code = 200200023, Message = "Type is required." });

            if (item != null && string.IsNullOrEmpty(item.Format))
                failureList.Add(new ValidationFailure() { Code = 200200024, Message = "Format is required." });

            if (item != null && string.IsNullOrEmpty(item.Size))
                failureList.Add(new ValidationFailure() { Code = 200200025, Message = "Size is required." });

            if (item != null && string.IsNullOrEmpty(item.Brand))
                failureList.Add(new ValidationFailure() { Code = 200200026, Message = "Brand is invalid." });

            if (item != null && string.IsNullOrEmpty(item.Series))
                failureList.Add(new ValidationFailure() { Code = 200200030, Message = "Series is invalid." });

            if (item != null && item.Status != null && (item.Status != Statuses.Pending && item.Status != Statuses.Wishlist && item.Status != Statuses.Owned && item.Status != Statuses.NotOwned))
                failureList.Add(new ValidationFailure() { Code = 200200034, Message = "Status selection is invalid." });

            if (item != null && item.Type != null && (item.Type != Types.Set && item.Type != Types.SoldSeparately && item.Type != Types.Blind))
                failureList.Add(new ValidationFailure() { Code = 200200035, Message = "Type selection is invalid." });
           
            if (item != null && item.Format != null && (item.Format != Formats.Plush && item.Format != Formats.Figure && item.Format != Formats.Keychain && item.Format != Formats.Other))
                failureList.Add(new ValidationFailure() { Code = 200200036, Message = "Format selection is invalid." });

            if (item != null && item.Size != null && (item.Size != Sizes.Irregular && item.Size != Sizes.Large && item.Size != Sizes.Regular && item.Size != Sizes.Mini))
                failureList.Add(new ValidationFailure() { Code = 200200037, Message = "Size selection is invalid." });

            if (item != null && item.Name != null && item.Name.Length > 100)
                failureList.Add(new ValidationFailure() { Code = 200200038, Message = "Name is too long." });

            if (item != null && item.Description != null && item.Description.Length > 250)
                failureList.Add(new ValidationFailure() { Code = 200200039, Message = "Description is too long." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public string ValidateUpdateItem(Item item)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (item == null)
                failureList.Add(new ValidationFailure() { Code = 200200040, Message = "Item object is invalid." });

            if (item != null && item.Id == 0)
                failureList.Add(new ValidationFailure() { Code = 200200041, Message = "Item Id is invalid." });

            if (item != null && item.Id < 0)
                failureList.Add(new ValidationFailure() { Code = 200200042, Message = "Item Id is invalid." });

            if (item != null && item.Id > 99999)
                failureList.Add(new ValidationFailure() { Code = 200200043, Message = "Item Id is invalid." });

            if (item != null && !int.TryParse(item.Id.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 200200044, Message = "Item Id is invalid." });

            if (item != null && string.IsNullOrEmpty(item.Status))
                failureList.Add(new ValidationFailure() { Code = 200200046, Message = "Status is required." });

            if (item != null && string.IsNullOrEmpty(item.Type))
                failureList.Add(new ValidationFailure() { Code = 200200047, Message = "Type is required." });

            if (item != null && string.IsNullOrEmpty(item.Format))
                failureList.Add(new ValidationFailure() { Code = 200200048, Message = "Format is required." });

            if (item != null && string.IsNullOrEmpty(item.Size))
                failureList.Add(new ValidationFailure() { Code = 200200049, Message = "Size is required." });

            if (item != null && string.IsNullOrEmpty(item.Brand))
                failureList.Add(new ValidationFailure() { Code = 200200050, Message = "Brand is invalid." });

            if (item != null && string.IsNullOrEmpty(item.Series))
                failureList.Add(new ValidationFailure() { Code = 200200054, Message = "Series is invalid." });

            if (item != null && item.Status != null && (item.Status != Statuses.Pending && item.Status != Statuses.Wishlist && item.Status != Statuses.Owned && item.Status != Statuses.NotOwned))
                failureList.Add(new ValidationFailure() { Code = 200200058, Message = "Status selection is invalid." });

            if (item != null && item.Type != null && (item.Type != Types.Set && item.Type != Types.SoldSeparately && item.Type != Types.Blind))
                failureList.Add(new ValidationFailure() { Code = 200200059, Message = "Type selection is invalid." });

            if (item != null && item.Format != null && (item.Format != Formats.Plush && item.Format != Formats.Figure && item.Format != Formats.Keychain && item.Format != Formats.Other))
                failureList.Add(new ValidationFailure() { Code = 200200060, Message = "Format selection is invalid." });

            if (item != null && item.Size != null && (item.Size != Sizes.Irregular && item.Size != Sizes.Large && item.Size != Sizes.Regular && item.Size != Sizes.Mini))
                failureList.Add(new ValidationFailure() { Code = 200200061, Message = "Size selection is invalid." });

            if (item != null && item.Name != null && item.Name.Length > 100)
                failureList.Add(new ValidationFailure() { Code = 200200062, Message = "Name is too long." });

            if (item != null && item.Description != null && item.Description.Length > 250)
                failureList.Add(new ValidationFailure() { Code = 200200063, Message = "Description is too long." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public string ValidateItemId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (id == 0)
                failureList.Add(new ValidationFailure() { Code = 200200064, Message = "Item Id is invalid." });

            if (id < 0)
                failureList.Add(new ValidationFailure() { Code = 200200065, Message = "Item Id is invalid." });

            if (id > 99999)
                failureList.Add(new ValidationFailure() { Code = 200200066, Message = "Item Id is invalid." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }
    }
}
