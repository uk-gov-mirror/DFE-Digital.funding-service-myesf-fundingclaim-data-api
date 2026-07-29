using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pds.Core.Logging;
using Pds.FundingClaim.Repositories.DataModels;
using Pds.FundingClaim.Services.Interfaces;
using Pds.FundingClaim.Services.Models;
using Sfa.Sfs.Contracts.Messaging;
using System;
using System.Threading.Tasks;

namespace Pds.FundingClaim.Api.Controllers
{
    /// <summary>
    /// Api that supports CRUD operations on funding claim data.
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ReconciliationController : Controller
    {
        #region Private Fields

        private readonly ISettingDataService _settingDataService;
        private readonly IReconciliationDataService _reconciliationDataService;
        private readonly ILoggerAdapter<ReconciliationController> _logger;

        #endregion Private Fields


        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReconciliationController"/> class.
        /// </summary>
        /// <param name="settingDataService">Concrete class that implements <see cref=ISettingDataService"/>.</param>
        /// <param name="reconciliationDataService">Concrete class that implements <see cref=IReconciliationDataService"/>.</param>
        /// <param name="logger">The logger.</param>
        public ReconciliationController(
            ISettingDataService settingDataService,
            IReconciliationDataService reconciliationDataService,
            ILoggerAdapter<ReconciliationController> logger)
        {
            _settingDataService = settingDataService;
            _reconciliationDataService = reconciliationDataService;
            _logger = logger;
        }

        #endregion Public Constructors


        #region Api Methods

        /// <summary>
        /// Api method to get reconciliation By Id.
        /// </summary>
        /// <param name="reconciliationId">Reconciliation Id.</param>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpGet("{reconciliationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<Reconciliations> GetReconciliationById(int reconciliationId)
        {
            var reconciliationResult = await _reconciliationDataService.GetReconciliationById(reconciliationId);

            _logger.LogInformation($"{(reconciliationResult == null ? "Null" : $"Reconciliation with Id [{reconciliationResult.Id}]")} returned from GetReconciliationById using Id {reconciliationId}");

            return reconciliationResult;
        }

        /// <summary>
        /// Api method to get reconciliation feed bookmark id setting when requested.
        /// </summary>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<string> GetReconciliationFeedBookmarkIdSetting()
        {
            var settingResult = await _settingDataService.GetReconciliationFeedBookmarkIdSetting();
            _logger.LogInformation($"GetReconciliationFeedBookmarkIdSetting returned setting value: {settingResult}");

            return settingResult;
        }

        /// <summary>
        /// Api method to get reconciliation feed read warning threshold setting when requested.
        /// </summary>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<string> GetFeedReadWarningThresholdSetting()
        {
            var settingResult = await _settingDataService.GetFeedReadWarningThresholdSetting();
            _logger.LogInformation($"GetFeedReadWarningThresholdSetting returned setting value: {settingResult}");

            return settingResult;
        }

        /// <summary>
        /// Api method to get use new reconciliation feed reader setting when requested.
        /// </summary>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<string> GetUseNewReconciliationsFeedReaderSetting()
        {
            var settingResult = await _settingDataService.GetUseNewReconciliationsFeedReaderSetting();
            _logger.LogInformation($"GetUseNewReconciliationsFeedReaderSetting returned setting value: {settingResult}");

            return settingResult;
        }

        /// <summary>
        /// Api method to CreateReconciliation and send relevant emails.
        /// </summary>
        /// <param name="reconciliation">The reconciliation to be processed.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReconciliation(FeedReconciliation reconciliation)
        {
            await _reconciliationDataService.CreateReconciliation(reconciliation);
            _logger.LogInformation($"CreateReconciliation ran successfully.");

            return Created("CreateReconciliation", null);
        }

        /// <summary>
        /// Api method to audit reconciliation feed read Exception.
        /// </summary>
        /// <param name="message">The error message to be audited.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AuditReconciliationFeedReadException([FromBody] string message)
        {
            await _reconciliationDataService.AuditReconciliationFeedReadException(message);
            _logger.LogInformation(
            $"AuditReconciliationFeedReadException audited message: {message}.");

            return Created("AuditReconciliationFeedReadException", null);
        }

        /// <summary>
        /// Api method to send feed read exception email.
        /// </summary>
        /// <param name="message">The message content.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendFeedReadExceptionEmail(FeedReadExceptionMessage message)
        {
            await _reconciliationDataService.SendFeedReadExceptionEmail(message);
            _logger.LogInformation(
            $"SendFeedReadExceptionEmail sent message for type: {message.Type}.");

            return Ok();
        }

        /// <summary>
        /// Api method to send feed exceeded read threshold warning email.
        /// </summary>
        /// <param name="message">The message content.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendFeedExceededReadThresholdWarningEmail(FeedReadThresholdExceededWarningMessage message)
        {
            await _reconciliationDataService.SendFeedExceededReadThresholdWarningEmail(message);
            _logger.LogInformation(
            $"SendFeedExceededReadThresholdWarningEmail sent message for url: {message.LastPageUrl}.");

            return Ok();
        }


        /// <summary>
        /// Api method to update the book mark id.
        /// </summary>
        /// <param name="bookmarkId">The bookmark id to be updated.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateReconciliationFeedBookmarkId([FromBody] Guid bookmarkId)
        {
            await _reconciliationDataService.UpdateReconciliationFeedBookmarkId(bookmarkId);
            _logger.LogInformation(
            $"Updated the reconciliation feed bookmarkid: {bookmarkId}.");

            return Ok();
        }

        #endregion
    }
}