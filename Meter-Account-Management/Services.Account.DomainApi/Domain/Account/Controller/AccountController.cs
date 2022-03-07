using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Account.Common.Exceptions;
using Services.Account.DomainApi.Domain.Account.Processor;
using Services.Account.Management.Controllers;
using Services.Account.Surface;

namespace Services.Account.DomainApi.Domain.Account
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController: Controller
    {

        /// <summary>
        /// Logger object for Controller
        /// </summary>
        private ILogger<AccountController> _logger { get; set; }

        /// <summary>
        ///  Domain processor
        /// </summary>
        private IAccountProcessor _accountProcessor { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="applicantProcessor"></param>
        public AccountController(ILogger<AccountController> logger, IAccountProcessor accountProcessor)
        {
            _logger = logger;
            _accountProcessor = accountProcessor;
        }

        /// <summary>
        /// Return the list of meter readings
        /// </summary>
        /// <returns></returns>
        [HttpGet("getmeterreading")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        [ProducesResponseType(typeof(List<MeterDetails>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionPayload), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllMeterReading()
        {
            List<MeterDetails> meterDetails = null;
            try
            {
                _logger.LogInformation($"{nameof(GetAllMeterReading)} processing has been started");
                meterDetails = _accountProcessor.GetAllMeterReading();
                _logger.LogInformation($"{nameof(GetAllMeterReading)} processing has been completed");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"{ex.GetType().Name}: Error occurred during GetAllMeterReading call");
                throw;
            }
            return base.Ok(new List<MeterDetails>(meterDetails));
        }

        /// <summary>
        /// Import meter reading
        /// </summary>
        /// <returns></returns>
        [HttpPost("importmeterreading")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ImportMeterDetails()
        {
            string result = string.Empty;
            try
            {
                _logger.LogInformation($"{nameof(ImportMeterDetails)} processing has been started");
                result = await _accountProcessor.ImportMeterReading();
                _logger.LogInformation($"{nameof(ImportMeterDetails)} processing has been completed");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"{ex.GetType().Name}: Error occurred during ImportMeterDetails call");
                throw;
            }

            return Content(result);
        }
    }
}
