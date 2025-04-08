using ItemApi.Models.Classes.V1;
using ItemApi.Models.System;
using Microsoft.SqlServer.Server;
using System.Drawing;

namespace ItemApi.Validators.V1
{
    public interface ISeriesControllerValidatorV1
    {
        string ValidateAddSeries(Series series);
        string ValidateUpdateSeries(Series series);
    }

    public class SeriesControllerValidatorV1 : ISeriesControllerValidatorV1
    {
        public string ValidateAddSeries(Series series)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (series == null)
                failureList.Add(new ValidationFailure() { Code = 200200010, Message = "Series object is invalid." });

            if (series != null && string.IsNullOrEmpty(series.SeriesName))
                failureList.Add(new ValidationFailure() { Code = 200200075, Message = "Name is required." });

            if (series != null && series.SeriesName != null && series.SeriesName.Length > 50)
                failureList.Add(new ValidationFailure() { Code = 200200076, Message = "Name is too long." });

            if (series != null && series.Description != null && series.Description.Length > 250)
                failureList.Add(new ValidationFailure() { Code = 200200077, Message = "Description is too long." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public string ValidateUpdateSeries(Series series)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (series == null)
                failureList.Add(new ValidationFailure() { Code = 200200078, Message = "Series object is invalid." });

            if (series != null && series.Id == 0)
                failureList.Add(new ValidationFailure() { Code = 200200079, Message = "Series Id is invalid." });

            if (series != null && series.Id < 0)
                failureList.Add(new ValidationFailure() { Code = 200200080, Message = "Series Id is invalid." });

            if (series != null && series.Id > 99999)
                failureList.Add(new ValidationFailure() { Code = 200200081, Message = "Series Id is invalid." });

            if (series != null && !int.TryParse(series.Id.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 200200082, Message = "Series Id is invalid." });

            if (series != null && string.IsNullOrEmpty(series.SeriesName))
                failureList.Add(new ValidationFailure() { Code = 200200083, Message = "Name is required." });

            if (series != null && series.SeriesName != null && series.SeriesName.Length > 50)
                failureList.Add(new ValidationFailure() { Code = 200200084, Message = "Name is too long." });

            if (series != null && series.Description != null && series.Description.Length > 250)
                failureList.Add(new ValidationFailure() { Code = 200200085, Message = "Description is too long." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }
    }
}
