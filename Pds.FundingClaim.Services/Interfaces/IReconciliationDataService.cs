using Pds.FundingClaim.Repositories.DataModels;
using Pds.FundingClaim.Services.Models;
using Sfa.Sfs.Contracts.Messaging;
using System;
using System.Threading.Tasks;

namespace Pds.FundingClaim.Services.Interfaces
{
    /// <summary>
    /// Interface for Funding Claim related data operations.
    /// </summary>
    public interface IReconciliationDataService
    {
        /// <summary>
        /// Creates reconciliations in the database.
        /// </summary>
        /// <param name="reconciliation">List of reconciliations to be added.</param>
        /// <returns>The asynchronous task.</returns>
        Task CreateReconciliation(FeedReconciliation reconciliation);

        /// <summary>
        /// Audits reconciliation feed read exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <returns>The asynchronous task.</returns>
        Task AuditReconciliationFeedReadException(string message);

        /// <summary>
        /// Sends a message that an exception occured while reading reconciliation feed.
        /// </summary>
        /// <param name="message">The message content.</param>
        /// <returns>The asynchronous task.</returns>
        Task SendFeedReadExceptionEmail(FeedReadExceptionMessage message);

        /// <summary>
        /// Sends a message when threshold time has passed whilst reading reconciliation feed.
        /// </summary>
        /// <param name="message">The message content.</param>
        /// <returns>The asynchronous task.</returns>
        Task SendFeedExceededReadThresholdWarningEmail(FeedReadThresholdExceededWarningMessage message);

        /// <summary>
        /// Update reconciliation feed bookmarkId.
        /// </summary>
        /// <param name="bookmarkId">book mark id.</param>
        /// <returns>The asynchronous task.</returns>
        Task UpdateReconciliationFeedBookmarkId(Guid bookmarkId);

        /// <summary>
        /// Get Reconciliation By Id.
        /// </summary>
        /// <param name="reconciliationId">Reconciliation Id.</param>
        /// <returns>Reconciliation by Id.</returns>
        Task<Reconciliations> GetReconciliationById(int reconciliationId);
    }
}