using Pds.Audit.Api.Client.Interfaces;
using Pds.Core.Logging;
using Pds.FundingClaim.CorporateSchema.Reconciliations;
using Pds.FundingClaim.Repositories.DataModels;
using Pds.FundingClaim.Repositories.Enums;
using Pds.FundingClaim.Repositories.Enums.Support;
using Pds.FundingClaim.Repositories.Exceptions;
using Pds.FundingClaim.Repositories.Interfaces;
using Pds.FundingClaim.Services.Constants;
using Pds.FundingClaim.Services.Interfaces;
using Pds.FundingClaim.Services.Models;
using Sfa.Sfs.Contracts.Messaging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pds.FundingClaim.Services.Implementations
{
    /// <inheritdoc cref="IReconciliationDataService"/>
    public class ReconciliationDataService : IReconciliationDataService
    {
        #region Private Members

        private readonly ILoggerAdapter<ReconciliationDataService> _logger;
        private readonly IAuditService _auditService;
        private IRepository<Reconciliations> _reconciliationRepository;
        private IRepository<ReconciliationAllocationGroups> _reconciliationAllocationGroupsRepository;
        private IRepository<Setting> _settingsRepository;
        private ISystemProvider _systemProvider;
        private IEmailService _emailService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ReconciliationDataService"/> class.
        /// The parameterised constructor.
        /// </summary>
        /// <param name="reconciliationRepository">The reconciliation repository.</param>
        /// <param name="reconciliationAllocationGroupsRepository">The reconciliation allocation groups repository.</param>
        /// <param name="settingsRepository">The setting repository service.</param>
        /// <param name="logger">The logging service.</param>
        /// <param name="systemProvider">The System provider.</param>
        /// <param name="emailService">The email service.</param>
        /// <param name="auditService">The shared audit service.</param>
        public ReconciliationDataService(
            IRepository<Reconciliations> reconciliationRepository,
            IRepository<ReconciliationAllocationGroups> reconciliationAllocationGroupsRepository,
            IRepository<Setting> settingsRepository,
            ILoggerAdapter<ReconciliationDataService> logger,
            ISystemProvider systemProvider,
            IEmailService emailService,
            IAuditService auditService)
        {
            _reconciliationRepository = reconciliationRepository;
            _reconciliationAllocationGroupsRepository = reconciliationAllocationGroupsRepository;
            _settingsRepository = settingsRepository;
            _logger = logger;
            _systemProvider = systemProvider;
            _emailService = emailService;
            _auditService = auditService;
        }

        #endregion

        /// <inheritdoc/>
        public async Task CreateReconciliation(FeedReconciliation reconciliation)
        {
            try
            {
                await CreateReconciliationFromFeed(reconciliation.Reconciliation);
                var reconciliationAttributes = reconciliation.Reconciliation.FCReconciliationAllAttrs.FCReconciliationPKeyAttrs;
                await _auditService.AuditAsync(
                    new Audit.Api.Client.Models.Audit
                    {
                        Severity = Audit.Api.Client.Enumerations.SeverityLevel.Information,
                        Action = Audit.Api.Client.Enumerations.ActionType.ReconciliationFeedRead,
                        Ukprn = reconciliationAttributes.Contractor.ContractorNonPKeyAttrs.UKPRN,
                        //// Note that we don't have the user data.
                        User = "System",
                        Message = $"Reconciliation with details UKPRN [{reconciliationAttributes.Contractor.ContractorNonPKeyAttrs.UKPRN}] Version [{reconciliationAttributes.ClaimVersionNumber}] ClaimTypeName [{reconciliationAttributes.ClaimType.ClaimTypeNonPKeyAttrs.ClaimTypeName}] Period [{reconciliationAttributes.Period.PeriodValue}] has been created."
                    });
            }
            catch (ReconciliationAlreadyExistsForUkprnPeriodVersionAndTypeException exception)
            {
                var ukprn = reconciliation.Reconciliation.FCReconciliationAllAttrs.FCReconciliationPKeyAttrs.Contractor.ContractorNonPKeyAttrs.UKPRN;
                await _auditService.AuditAsync(
                    new Audit.Api.Client.Models.Audit
                    {
                        Severity = Audit.Api.Client.Enumerations.SeverityLevel.Error,
                        Action = Audit.Api.Client.Enumerations.ActionType.ReconciliationFeedRead,
                        Ukprn = ukprn,
                        //// Note that we don't have the user data.
                        User = "System",
                        Message = exception.Message.TrimEnd()
                    });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task AuditReconciliationFeedReadException(string message)
        {
            await _auditService.AuditAsync(
                   new Audit.Api.Client.Models.Audit
                   {
                       Severity = Audit.Api.Client.Enumerations.SeverityLevel.Error,
                       Action = Audit.Api.Client.Enumerations.ActionType.ReconciliationFeedRead,
                       Ukprn = null,
                       //// Note that we don't have the user data.
                       User = "System",
                       Message = message
                   });
        }

        /// <inheritdoc/>
        public async Task SendFeedReadExceptionEmail(FeedReadExceptionMessage message)
        {
            await _emailService.SendFeedReadExceptionEmail(message);
        }

        /// <inheritdoc/>
        public async Task SendFeedExceededReadThresholdWarningEmail(FeedReadThresholdExceededWarningMessage message)
        {
            await _emailService.SendFeedExceededReadThresholdWarningEmail(message);

            var thresholdReachedMessage = $"Reconciliation Feed read threshold exceeded whilst looking for {message.BookmarkId}. Read started at {message.Start}, warning raised at {message.Now}. Last page read was {message.LastPageUrl}.";

            await _auditService.AuditAsync(
                  new Audit.Api.Client.Models.Audit
                  {
                      Severity = Audit.Api.Client.Enumerations.SeverityLevel.Warning,
                      Action = Audit.Api.Client.Enumerations.ActionType.ReconciliationFeedRead,
                      Ukprn = null,
                      //// Note that we don't have the user data.
                      User = "System",
                      Message = thresholdReachedMessage
                  });
        }

        /// <summary>
        /// Updates the ReconciliationFeedBookmarkIdSetting database.
        /// </summary>
        /// <param name="bookmarkId">The value to be updated.</param>
        /// <returns>The asynchronous Task.</returns>
        public async Task UpdateReconciliationFeedBookmarkId(Guid bookmarkId)
        {
            try
            {
                var reconciliationFeedBookmarkIdSetting = await _settingsRepository.FirstOrDefault(setting => setting.Type == ServiceConstants.ReconciliationFeedBookmarkIdSetting);

                reconciliationFeedBookmarkIdSetting.Value = bookmarkId.ToString();
                reconciliationFeedBookmarkIdSetting.UpdatedAt = DateTime.UtcNow;
                await _settingsRepository.Update(reconciliationFeedBookmarkIdSetting);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
            }
        }

        /// <inheritdoc/>
        public async Task<Reconciliations> GetReconciliationById(int reconciliationId)
        {
            try
            {
                return await _reconciliationRepository.FirstOrDefault(reconciliation => reconciliation.Id == reconciliationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }


        #region Private Methods

        /// <summary>
        /// Creates a Funding Claim Reconciliation.
        /// </summary>
        /// <param name="reconciliationToProcess">The funding claim reconciliation to process.</param>
        /// <returns>The reconciliation entity created.</returns>
        private async Task<Reconciliations> CreateReconciliationFromFeed(FCReconciliation reconciliationToProcess)
        {
            var reconciliationAttributes = reconciliationToProcess.FCReconciliationAllAttrs.FCReconciliationPKeyAttrs;

            var ukprn = reconciliationAttributes.Contractor.ContractorNonPKeyAttrs.UKPRN;
            var version = reconciliationAttributes.ClaimVersionNumber;
            var claimTypeName = reconciliationAttributes.ClaimType.ClaimTypeNonPKeyAttrs.ClaimTypeName;
            var period = reconciliationAttributes.Period.PeriodValue;
            var allocationGroupCode = reconciliationAttributes.AllocationGroup.AllocationGroupPKeyAttrs.AllocationGroupCode;

            var reconciliationType = claimTypeName.ToReconciliationType();

            if (ukprn == 0)
            {
                throw new InvalidDataException($"UKPRN is not found for {reconciliationAttributes.Contractor.ContractorPKeyAttrs.OrganisationIdentifier}.");
            }

            var reconciliationData = new ReconciliationData(reconciliationToProcess);

            var reconciliation = new Reconciliations(reconciliationData, ukprn, version, reconciliationType, period, SetTitle(allocationGroupCode, period, reconciliationType));

            EnsureNoMatchingExists(reconciliation);

            reconciliation.IsValid = true;
            reconciliation.CreatedAt = reconciliation.LastUpdatedAt = _systemProvider.UtcNow();

            var createdReconciliation = await _reconciliationRepository.Create(reconciliation);

            LogProcessedReconciliation(reconciliationToProcess, reconciliation.IsValid.Value);

            await _emailService.SendReconciliationReadyToBeViewedEmail(createdReconciliation.Id);

            return createdReconciliation;
        }

        /// <summary>
        /// Checks if the same reconciliation already exists.
        /// </summary>
        /// <param name="reconciliationToProcess">The reconciliation to match on.</param>
        private void EnsureNoMatchingExists(Reconciliations reconciliationToProcess)
        {
            var any = _reconciliationRepository.Where(reconciliation =>
                       reconciliation.Ukprn == reconciliationToProcess.Ukprn
                       && reconciliation.Period == reconciliationToProcess.Period
                       && reconciliation.Version == reconciliationToProcess.Version
                       && reconciliation.Title == reconciliationToProcess.Title
                       && reconciliation.Type == reconciliationToProcess.Type).Any();

            if (any)
            {
                throw new ReconciliationAlreadyExistsForUkprnPeriodVersionAndTypeException(reconciliationToProcess);
            }
        }

        /// <summary>
        /// Adds appropriate log message for reconciliation created.
        /// </summary>
        /// <param name="reconciliationProcessed">The reconciliation processed.</param>
        /// <param name="isValid">Whether reconciliation is valid or not.</param>
        private void LogProcessedReconciliation(FCReconciliation reconciliationProcessed, bool isValid)
        {
            var message = $"The reconciliation with Organisation Identifier: {reconciliationProcessed.FCReconciliationAllAttrs.FCReconciliationPKeyAttrs.Contractor.ContractorPKeyAttrs.OrganisationIdentifier}, ContractVersion: {reconciliationProcessed.FCReconciliationAllAttrs.FCReconciliationPKeyAttrs.ClaimVersionNumber}, period: {reconciliationProcessed.FCReconciliationAllAttrs.FCReconciliationPKeyAttrs.Period.PeriodValue}, ClaimTypeCode {reconciliationProcessed.FCReconciliationAllAttrs.FCReconciliationPKeyAttrs.ClaimType.ClaimTypePKeyAttrs.ClaimTypeCode}, ClaimTypeName: {reconciliationProcessed.FCReconciliationAllAttrs.FCReconciliationPKeyAttrs.ClaimType.ClaimTypeNonPKeyAttrs.ClaimTypeName} was processed for {reconciliationProcessed.FCReconciliationAllAttrs.FCReconciliationPKeyAttrs.ClaimType.ClaimTypeNonPKeyAttrs.ClaimTypeName} successfully.";

            _logger.LogInformation(message);
        }

        /// <summary>
        /// Sets the title of the reconciliation statement.
        /// </summary>
        /// <param name="groupCode">The allocation group code.</param>
        /// <param name="period">The period.</param>
        /// <param name="type">The reconciliation type.</param>
        /// <returns>The title determined.</returns>
        private string SetTitle(string groupCode, string period, ReconciliationType type)
        {
            var periodStart = period.Substring(0, 2);
            var periodEnd = period.Substring(2);

            var groupDetails = "Unknown";
            var group = _reconciliationAllocationGroupsRepository.Where(group => group.Code == groupCode);

            if (group.Any())
            {
                groupDetails = group.Single().Description;
            }

            return $"{type.GetDisplayName()} reconciliation for {groupDetails} for 20{periodStart} to 20{periodEnd}";
        }

        #endregion
    }
}