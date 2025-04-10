using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SeriesApi.Models.Classes.V1;
using SeriesApi.Models.System;
using SeriesApi.Data.Validators.V1;

namespace SeriesApi.Workflows.Validators.V1
{
    public interface ISeriesWorkflowValidatorV1
    {
        Task<string> ValidateAddSeries(Series series);
        Task<string> ValidateUpdateSeries(Series series);
        Task<string> ValidateSeriesId(int id);
    }

    public class SeriesWorkflowValidatorV1 : ISeriesWorkflowValidatorV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ISeriesDataValidatorV1 _dataValidator;

        public SeriesWorkflowValidatorV1(ILoggerFactory loggerFactory, IConfiguration configuration, ISeriesDataValidatorV1 dataValidator)
        {
            _logger = loggerFactory.CreateLogger<SeriesWorkflowValidatorV1>();
            _configuration = configuration;
            _dataValidator = dataValidator;
        }

        public async Task<string> ValidateAddSeries(Series series)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (series == null)
                failureList.Add(new ValidationFailure() { Code = 400400001, Message = "Series object is invalid." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateUpdateSeries(Series series)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (series == null)
                failureList.Add(new ValidationFailure() { Code = 400400002, Message = "Series object is invalid." });

            if (series != null)
            {
                bool seriesExists = await ValidateSeriesExists(series.Id);
                if (!seriesExists) failureList.Add(new ValidationFailure() { Code = 400400003, Message = "Series does not exist." });
            }

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateSeriesId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            bool seriesExists = await ValidateSeriesExists(id);
            if (!seriesExists) failureList.Add(new ValidationFailure() { Code = 400400004, Message = "Series does not exist." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        private async Task<bool> ValidateSeriesExists(int id)
        {
            bool valid = await _dataValidator.VerifySeries(id);
            return valid;
        }


    }
}
