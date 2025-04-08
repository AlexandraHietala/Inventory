using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using UserApi.Models.DTOs.V1;

namespace UserApi.Data.DataOperations.V1
{
    public interface IRoleOperationsV1
    {
        Task<RoleDto> GetRole(int id);
        Task<List<RoleDto>> GetRoles();
    }

    public class GetRoleOperationsV1 : IRoleOperationsV1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connString;

        public GetRoleOperationsV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetRoleOperationsV1>();
            _configuration = configuration;
            _connString = _configuration.GetConnectionString("StarryEdenUser")!;
        }

        public async Task<RoleDto> GetRole(int id)
        {
            try
            {
                _logger.LogDebug("GetRole request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                RoleDto role = await connection.QueryFirstAsync<RoleDto>("[app].[spGetRole]", new { id }, commandType: CommandType.StoredProcedure);

                _logger.LogInformation("GetRole success response.");
                return role;
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This role doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetRole InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[100500007] Role does not exist.");
                }
                else
                {
                    _logger.LogError($"[100500008] GetRole InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[100500009] GetRole Exception: {e}.");
                throw;
            }
        }

        public async Task<List<RoleDto>> GetRoles()
        {
            try
            {
                _logger.LogDebug("GetRoles request received.");

                using IDbConnection connection = new SqlConnection(_connString);
                IEnumerable<RoleDto> roles = await connection.QueryAsync<RoleDto>("[app].[spGetRoles]", new { }, commandType: CommandType.StoredProcedure);
                _logger.LogInformation("GetRoles success response.");
                return roles.ToList();
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message == "Sequence contains no elements")
                {
                    // This role doesn't exist and we somehow missed it on validation
                    _logger.LogInformation($"GetRoles InvalidOperationException: {ioe}. Rethrowing as ArgumentException.");
                    throw new ArgumentException("[100500010] Roles do not exist.");
                }
                else
                {
                    _logger.LogError($"[100500011] GetRoles InvalidOperationException: {ioe}.");
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"[100500012] GetRoles Exception: {e}.");
                throw;
            }
        }
    }
}
