using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ItemApi.Data.DataOperations.V1;
using ItemApi.Models.Classes.V1;
using ItemApi.Models.System;

namespace ItemApi.Workflows.Validators.V1
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
        private readonly IVerifyOperationsV1 _verifyOperations;

        public SeriesWorkflowValidatorV1(ILoggerFactory loggerFactory, IConfiguration configuration, IVerifyOperationsV1 verifyOperations)
        {
            _logger = loggerFactory.CreateLogger<SeriesWorkflowValidatorV1>();
            _configuration = configuration;
            _verifyOperations = verifyOperations;
        }

        public async Task<string> ValidateAddSeries(Series series)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (series == null)
                failureList.Add(new ValidationFailure() { Code = 200400013, Message = "Series object is invalid." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateUpdateSeries(Series series)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            if (series == null)
                failureList.Add(new ValidationFailure() { Code = 200400014, Message = "Series object is invalid." });

            if (series != null)
            {
                bool seriesExists = await ValidateSeriesExists(series.Id);
                if (!seriesExists) failureList.Add(new ValidationFailure() { Code = 200400015, Message = "Series does not exist." });
            }

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        public async Task<string> ValidateSeriesId(int id)
        {
            List<ValidationFailure> failureList = new List<ValidationFailure>();

            bool seriesExists = await ValidateSeriesExists(id);
            if (!seriesExists) failureList.Add(new ValidationFailure() { Code = 200400016, Message = "Series does not exist." });

            string failures = string.Empty;
            foreach (var failure in failureList) failures = failures + "[" + failure.Code.ToString() + "] " + failure.Message + " ";
            return failures;
        }

        private async Task<bool> ValidateSeriesExists(int id)
        {
            bool valid = await _verifyOperations.VerifySeries(id);
            return valid;
        }


    }
}
