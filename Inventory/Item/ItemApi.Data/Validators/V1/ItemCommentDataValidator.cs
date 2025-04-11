using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using ItemApi.Models.DTOs.V1;

namespace ItemApi.Data.Validators.V1
{
    public interface IItemCommentDataValidatorV1
    {
        Task<bool> VerifyItemComment(int id);
    }

    public class ItemCommentDataValidatorV1 : IItemCommentDataValidatorV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public ItemCommentDataValidatorV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<ItemCommentDataValidatorV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("SEInventory")!;
        }

        public async Task<bool> VerifyItemComment(int id)
        {
            try
            {
                _logger.LogDebug("VerifyItemComment request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                ItemCommentDto comment = await connection.QueryFirstAsync<ItemCommentDto>("[app].[spGetItemComment]", new { id = id }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("VerifyItemComment success response.");
                if (comment != null) return true;
                else return false;
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogDebug($"VerifyItemComment InvalidOperationException: {ioe}. Suppressing exception and returning false.");
                return false;
            }
            catch (Exception e)
            {
                _logger.LogDebug($"VerifyItemComment Exception: {e}. Suppressing exception and returning false.");
                return false;
            }
        }
    }
}
