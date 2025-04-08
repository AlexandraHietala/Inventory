using ItemApi.Models.Classes.V1;
using ItemApi.Models.System;
using Microsoft.SqlServer.Server;
using System.Drawing;

namespace ItemApi.Validators.V1
{
    public interface IItemControllerValidatorV1
    {
        string ValidateAddItem(Item item);
        string ValidateUpdateItem(Item item);
    }

    public class ItemControllerValidatorV1 : IItemControllerValidatorV1
    {
        public string ValidateAddItem(Item item)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (item == null)
                failureList.Add(new ValidationFailure() { Code = 200200033, Message = "Item object is invalid." });

            if (item != null && string.IsNullOrEmpty(item.Status))
                failureList.Add(new ValidationFailure() { Code = 200200034, Message = "Status is required." });

            if (item != null && string.IsNullOrEmpty(item.Type))
                failureList.Add(new ValidationFailure() { Code = 200200035, Message = "Type is required." });

            if (item != null && string.IsNullOrEmpty(item.Format))
                failureList.Add(new ValidationFailure() { Code = 200200036, Message = "Format is required." });

            if (item != null && string.IsNullOrEmpty(item.Size))
                failureList.Add(new ValidationFailure() { Code = 200200037, Message = "Size is required." });

            if (item != null && item.BrandId != null && item.BrandId == 0)
                failureList.Add(new ValidationFailure() { Code = 200200038, Message = "Brand is invalid." });

            if (item != null && item.BrandId != null && item.BrandId < 1)
                failureList.Add(new ValidationFailure() { Code = 200200039, Message = "Brand is invalid." });

            if (item != null && item.BrandId != null && !int.TryParse(item.BrandId.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 200200040, Message = "Brand is invalid." });

            if (item != null && item.BrandId != null && item.BrandId > 99999)
                failureList.Add(new ValidationFailure() { Code = 200200041, Message = "Brand is invalid." });

            if (item != null && item.SeriesId != null && item.SeriesId == 0)
                failureList.Add(new ValidationFailure() { Code = 200200042, Message = "Series is invalid." });

            if (item != null && item.SeriesId != null && item.SeriesId < 1)
                failureList.Add(new ValidationFailure() { Code = 200200043, Message = "Series is invalid." });

            if (item != null && item.SeriesId != null && !int.TryParse(item.SeriesId.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 200200044, Message = "Series is invalid." });

            if (item != null && item.SeriesId != null && item.SeriesId > 99999)
                failureList.Add(new ValidationFailure() { Code = 200200045, Message = "Series is invalid." });

            if (item != null && item.Status != null && (item.Status != Statuses.Pending && item.Status != Statuses.Wishlist && item.Status != Statuses.Owned && item.Status != Statuses.NotOwned))
                failureList.Add(new ValidationFailure() { Code = 200200046, Message = "Status selection is invalid." });

            if (item != null && item.Type != null && (item.Type != Types.Set && item.Type != Types.SoldSeparately && item.Type != Types.Blind))
                failureList.Add(new ValidationFailure() { Code = 200200047, Message = "Type selection is invalid." });
           
            if (item != null && item.Format != null && (item.Format != Formats.Plush && item.Format != Formats.Figure && item.Format != Formats.Keychain && item.Format != Formats.Other))
                failureList.Add(new ValidationFailure() { Code = 200200048, Message = "Format selection is invalid." });

            if (item != null && item.Size != null && (item.Size != Sizes.Irregular && item.Size != Sizes.Large && item.Size != Sizes.Regular && item.Size != Sizes.Mini))
                failureList.Add(new ValidationFailure() { Code = 200200049, Message = "Size selection is invalid." });

            if (item != null && item.Name != null && item.Name.Length > 100)
                failureList.Add(new ValidationFailure() { Code = 200200050, Message = "Name is too long." });

            if (item != null && item.Description != null && item.Description.Length > 250)
                failureList.Add(new ValidationFailure() { Code = 200200051, Message = "Description is too long." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public string ValidateUpdateItem(Item item)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (item == null)
                failureList.Add(new ValidationFailure() { Code = 200200052, Message = "Item object is invalid." });

            if (item != null && item.Id == 0)
                failureList.Add(new ValidationFailure() { Code = 200200053, Message = "Item Id is invalid." });

            if (item != null && item.Id < 0)
                failureList.Add(new ValidationFailure() { Code = 200200054, Message = "Item Id is invalid." });

            if (item != null && item.Id > 99999)
                failureList.Add(new ValidationFailure() { Code = 200200055, Message = "Item Id is invalid." });

            if (item != null && !int.TryParse(item.Id.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 200200056, Message = "Item Id is invalid." });

            if (item != null && string.IsNullOrEmpty(item.Status))
                failureList.Add(new ValidationFailure() { Code = 200200057, Message = "Status is required." });

            if (item != null && string.IsNullOrEmpty(item.Type))
                failureList.Add(new ValidationFailure() { Code = 200200058, Message = "Type is required." });

            if (item != null && string.IsNullOrEmpty(item.Format))
                failureList.Add(new ValidationFailure() { Code = 200200059, Message = "Format is required." });

            if (item != null && string.IsNullOrEmpty(item.Size))
                failureList.Add(new ValidationFailure() { Code = 200200060, Message = "Size is required." });

            if (item != null && item.BrandId != null && item.BrandId == 0)
                failureList.Add(new ValidationFailure() { Code = 200200061, Message = "Brand is invalid." });

            if (item != null && item.BrandId != null && item.BrandId < 1)
                failureList.Add(new ValidationFailure() { Code = 200200062, Message = "Brand is invalid." });

            if (item != null && item.BrandId != null && !int.TryParse(item.BrandId.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 200200063, Message = "Brand is invalid." });

            if (item != null && item.BrandId != null && item.BrandId > 99999)
                failureList.Add(new ValidationFailure() { Code = 200200064, Message = "Brand is invalid." });

            if (item != null && item.SeriesId != null && item.SeriesId == 0)
                failureList.Add(new ValidationFailure() { Code = 200200065, Message = "Series is invalid." });

            if (item != null && item.SeriesId != null && item.SeriesId < 1)
                failureList.Add(new ValidationFailure() { Code = 200200066, Message = "Series is invalid." });

            if (item != null && item.SeriesId != null && !int.TryParse(item.SeriesId.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 200200067, Message = "Series is invalid." });

            if (item != null && item.SeriesId != null && item.SeriesId > 99999)
                failureList.Add(new ValidationFailure() { Code = 200200068, Message = "Series is invalid." });

            if (item != null && item.Status != null && (item.Status != Statuses.Pending && item.Status != Statuses.Wishlist && item.Status != Statuses.Owned && item.Status != Statuses.NotOwned))
                failureList.Add(new ValidationFailure() { Code = 200200069, Message = "Status selection is invalid." });

            if (item != null && item.Type != null && (item.Type != Types.Set && item.Type != Types.SoldSeparately && item.Type != Types.Blind))
                failureList.Add(new ValidationFailure() { Code = 200200070, Message = "Type selection is invalid." });

            if (item != null && item.Format != null && (item.Format != Formats.Plush && item.Format != Formats.Figure && item.Format != Formats.Keychain && item.Format != Formats.Other))
                failureList.Add(new ValidationFailure() { Code = 200200071, Message = "Format selection is invalid." });

            if (item != null && item.Size != null && (item.Size != Sizes.Irregular && item.Size != Sizes.Large && item.Size != Sizes.Regular && item.Size != Sizes.Mini))
                failureList.Add(new ValidationFailure() { Code = 200200072, Message = "Size selection is invalid." });

            if (item != null && item.Name != null && item.Name.Length > 100)
                failureList.Add(new ValidationFailure() { Code = 200200073, Message = "Name is too long." });

            if (item != null && item.Description != null && item.Description.Length > 250)
                failureList.Add(new ValidationFailure() { Code = 200200074, Message = "Description is too long." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }
    }
}
