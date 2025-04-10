using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using ItemApi.Models.DTOs.V1;

namespace ItemApi.Data.DataOperations.V1
{
    public interface IUpdateItemCommentOperationsV1
    {
        Task UpdateItemComment(ItemCommentDto comment);
    }

    public class UpdateItemCommentOperationsV1 : IUpdateItemCommentOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public UpdateItemCommentOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateItemCommentOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenItem")!;
        }

        public async Task UpdateItemComment(ItemCommentDto comment)
        {
            try
            {
                _logger.LogDebug("UpdateItemComment request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                await connection.QueryFirstAsync<bool>("[app].[spUpdateItemComment]", new { id = comment.COMMENT_ID, item_id = comment.ITEM_ID, comment = comment.COMMENT, lastmodifiedby = comment.COMMENT_LAST_MODIFIED_BY }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("UpdateItemComment success response.");
                return;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    _logger.LogError($"[200500028] UpdateItemComment Error while updating comment: {ioe}");
                    throw;
                }
                else
                {
                    _logger.LogError($"[200500029] UpdateItemComment InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500030] UpdateItemComment Exception: {e}.");
                throw;
            }
        }
    }
}
