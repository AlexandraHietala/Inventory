using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using ItemApi.Models.DTOs.V1;

namespace ItemApi.Data.DataOperations.V1
{
    public interface IAddItemCommentOperationsV1
    {
        Task<int> AddItemComment(ItemCommentDto comment);
    }

    public class AddItemCommentOperationsV1 : IAddItemCommentOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public AddItemCommentOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddItemCommentOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenItem")!;
        }

        public async Task<int> AddItemComment(ItemCommentDto comment)
        {
            try
            {
                _logger.LogDebug("AddItemComment request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                int id = await connection.QueryFirstAsync<int>("[app].[spAddItemComment]", new { item_id = comment.ITEM_ID, comment = comment.COMMENT, lastmodifiedby = comment.COMMENT_LAST_MODIFIED_BY }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("AddItemComment success response.");
                return id;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[200500016] AddItemComment Error while inserting comment: {ioe}");
                    throw;
                }
                else
                {
                    _logger.LogError($"[200500017] AddItemComment InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500018] AddItemComment Exception: {e}.");
                throw;
            }
        }
    }
}
