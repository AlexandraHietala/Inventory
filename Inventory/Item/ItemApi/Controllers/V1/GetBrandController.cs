using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using Asp.Versioning;
using ItemApi.Validators.V1;
using ItemApi.Workflows.Workflows.V1;
using ItemApi.Models.Classes.V1;

namespace ItemApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("GetBrandControllerV1")]
    [Route("api/v{version:apiVersion}/brand")]
    public class GetBrandControllerV1 : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IGeneralControllerValidatorV1 _controllerValidator;
        private readonly IGetBrandWorkflowV1 _getBrandWorkflow;

        public GetBrandControllerV1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetBrandControllerV1>();
            _configuration = configuration;
            _controllerValidator = new GeneralControllerValidatorV1();
            _getBrandWorkflow = new GetBrandWorkflowV1(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getbrand")]
        public async Task<IActionResult> GetBrandV1(int id)
        {
            _logger.LogDebug("GetBrand request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                Brand brand = await _getBrandWorkflow.GetBrand(id);

                // Respond
                _logger.LogInformation("GetBrand success response.");
                return Ok(brand);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100013] GetBrand ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100014] GetBrand InvalidOperationException: {ioe}.");
                return NotFound("[200100014] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100015] GetBrand Exception: {e}.");
                return Problem("[200100015] " + e.Message);
            }
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getbrands")]
        public async Task<IActionResult> GetBrandsV1()
        {
            _logger.LogDebug("GetBrands request received.");

            try
            {
                // Validate
                // Nothing to validate

                // Process
                List<Brand> brands = await _getBrandWorkflow.GetBrands();

                // Respond
                _logger.LogInformation("GetBrands success response.");
                return Ok(brands);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100016] GetBrands ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100017] GetBrands InvalidOperationException: {ioe}.");
                return NotFound("[200100017] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100018] GetBrands Exception: {e}.");
                return Problem("[200100018] " + e.Message);
            }
        }
    }
}
