using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using ItemApi.Models.DTOs.V1;

namespace ItemApi.Data.DataOperations.V1
{
    public interface IRemoveItemCommentOperationsV1
    {
        Task RemoveItemComment(int id, string lastmodifiedby);
    }

    public class RemoveItemCommentOperationsV1 : IRemoveItemCommentOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public RemoveItemCommentOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveItemCommentOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenItem")!;
        }

        public async Task RemoveItemComment(int id, string lastmodifiedby)
        {
            try
            {
                _logger.LogDebug("RemoveItemComment request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                await connection.QueryFirstAsync<bool>("[app].[spRemoveItemComment]", new { id = id, lastmodifiedby = lastmodifiedby }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("RemoveItemComment success response.");
                return;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[200500046] Error while removing comment: {ioe}.");
                    throw;
                }
                else
                {
                    _logger.LogError($"[200500047] RemoveItemComment InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500048] RemoveItemComment Exception: {e}.");
                throw;
            }
        }

    }
}
