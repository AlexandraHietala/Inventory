using SeriesApi.Models.Classes.V1;
using SeriesApi.Models.System;
using Microsoft.SqlServer.Server;
using System.Drawing;

namespace SeriesApi.Validators.V1
{
    public interface ISeriesControllerValidatorV1
    {
        string ValidateSeriesId(int id);
        string ValidateAddSeries(Series series);
        string ValidateUpdateSeries(Series series);
    }

    public class SeriesControllerValidatorV1 : ISeriesControllerValidatorV1
    {
        public string ValidateAddSeries(Series series)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (series == null)
                failureList.Add(new ValidationFailure() { Code = 400200001, Message = "Series object is invalid." });

            if (series != null && string.IsNullOrEmpty(series.SeriesName))
                failureList.Add(new ValidationFailure() { Code = 400200002, Message = "Name is required." });

            if (series != null && series.SeriesName != null && series.SeriesName.Length > 50)
                failureList.Add(new ValidationFailure() { Code = 400200003, Message = "Name is too long." });

            if (series != null && series.Description != null && series.Description.Length > 250)
                failureList.Add(new ValidationFailure() { Code = 400200004, Message = "Description is too long." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public string ValidateUpdateSeries(Series series)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (series == null)
                failureList.Add(new ValidationFailure() { Code = 400200005, Message = "Series object is invalid." });

            if (series != null && series.Id == 0)
                failureList.Add(new ValidationFailure() { Code = 400200006, Message = "Series Id is invalid." });

            if (series != null && series.Id < 0)
                failureList.Add(new ValidationFailure() { Code = 400200007, Message = "Series Id is invalid." });

            if (series != null && series.Id > 99999)
                failureList.Add(new ValidationFailure() { Code = 400200008, Message = "Series Id is invalid." });

            if (series != null && !int.TryParse(series.Id.ToString(), out _))
                failureList.Add(new ValidationFailure() { Code = 400200009, Message = "Series Id is invalid." });

            if (series != null && string.IsNullOrEmpty(series.SeriesName))
                failureList.Add(new ValidationFailure() { Code = 400200010, Message = "Name is required." });

            if (series != null && series.SeriesName != null && series.SeriesName.Length > 50)
                failureList.Add(new ValidationFailure() { Code = 400200011, Message = "Name is too long." });

            if (series != null && series.Description != null && series.Description.Length > 250)
                failureList.Add(new ValidationFailure() { Code = 400200012, Message = "Description is too long." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public string ValidateSeriesId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (id == 0)
                failureList.Add(new ValidationFailure() { Code = 400200013, Message = "Series Id is invalid." });

            if (id < 0)
                failureList.Add(new ValidationFailure() { Code = 400200014, Message = "Series Id is invalid." });

            if (id > 99999)
                failureList.Add(new ValidationFailure() { Code = 400200015, Message = "Series Id is invalid." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }
    }
}
