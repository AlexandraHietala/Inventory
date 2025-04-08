using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using ItemApi.Models.DTOs.V1;
using Azure.Identity;

namespace ItemApi.Data.DataOperations.V1
{
    public interface IGetItemCommentOperationsV1
    {
        Task<ItemCommentDto> GetItemComment(int id);
        Task<List<ItemCommentDto>> GetItemComments(int itemId);
    }

    public class GetItemCommentOperationsV1 : IGetItemCommentOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public GetItemCommentOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetItemCommentOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenItem")!;
        }

        public async Task<ItemCommentDto> GetItemComment(int id)
        {
            try
            {
                _logger.LogDebug("GetItemComment request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                ItemCommentDto comment = await connection.QueryFirstAsync<ItemCommentDto>("[app].[spGetItemComment]", new { id = id }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("GetItemComment success response.");
                return comment;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This comment doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetItemComment InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[200500028] ItemComment does not exist.");
                }
                else
                {
                    _logger.LogError($"[200500029] GetItemComment InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500030] GetItemComment Exception: {e}.");
                throw;
            }
        }
        public async Task<List<ItemCommentDto>> GetItemComments(int itemId)
        {
            try
            {
                _logger.LogDebug("GetItemComments request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                IEnumerable<ItemCommentDto> comments = await connection.QueryAsync<ItemCommentDto>("[app].[spGetItemComments]", new { item_id = itemId }, commandType: CommandType.StoredProcedure);
                _logger.LogInformation("GetItemComments success response.");
                return comments.ToList();
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This comment doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetItemComments InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[200500031] ItemComments do not exist.");
                }
                else
                {
                    _logger.LogError($"[200500032] GetItemComments InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[200500033] GetItemComments Exception: {e}.");
                throw;
            }
        }
    }
}
