using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using SeriesApi.Models.DTOs.V1;

namespace SeriesApi.Data.DataOperations.V1
{
    public interface IVerifyOperationsV1
    {
        Task<bool> VerifySeries(int id);
    }

    public class VerifyOperationsV1 : IVerifyOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public VerifyOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<VerifyOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenItem")!;
        }

        public async Task<bool> VerifySeries(int id)
        {
            try
            {
                _logger.LogDebug("VerifySeries request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                SeriesDto series = await connection.QueryFirstAsync<SeriesDto>("[app].[spGetASeries]", new { id = id }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("VerifySeries success response.");
                if (series != null) return true;
                else return false;
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogDebug($"VerifySeries InvalidOperationException: {ioe}. Suppressing exception and returning false.");
                return false;
            }
            catch (Exception e)
            {
                _logger.LogDebug($"VerifySeries Exception: {e}. Suppressing exception and returning false.");
                return false;
            }
        }
    }
}
