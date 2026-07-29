using System.Threading.Tasks;

namespace Pds.FundingClaim.Services.Interfaces
{
    /// <summary>
    /// Interface for Settings related data operations.
    /// </summary>
    public interface ISettingDataService
    {
        /// <summary>
        /// Gets the Funding Claim Last Retrieved Setting from repository.
        /// </summary>
        /// <returns>The Funding Claim Last Retrieved Setting.</returns>
        Task<string> GetFundingClaimLastRetrievedSetting();

        /// <summary>
        /// Gets the Funding Claim Polling Setting from repository.
        /// </summary>
        /// <returns>The Funding Claim Polling Setting.</returns>
        Task<string> GetFundingClaimPollingSetting();

        /// <summary>
        /// Gets the Reconciliation Feed BookmarkId from repository.
        /// </summary>
        /// <returns>The ReconciliationFeedBookmarkId Setting.</returns>
        Task<string> GetReconciliationFeedBookmarkIdSetting();

        /// <summary>
        /// Gets the Reconciliation Feed BookmarkId from repository.
        /// </summary>
        /// <returns>The ReconciliationFeedBookmarkId Setting.</returns>
        Task<string> GetFeedReadWarningThresholdSetting();

        /// <summary>
        /// Gets the Use Json Format Of FundingClaims Setting from repository.
        /// </summary>
        /// <returns>The Use Json Format Of FundingClaims Setting.</returns>
        Task<string> GetUseJsonFormatOfFundingClaimsSetting();

        /// <summary>
        /// Gets the Use New Reconciliations FeedReader Setting from repository.
        /// </summary>
        /// <returns>The Use New Reconciliations FeedReader Setting.</returns>
        Task<string> GetUseNewReconciliationsFeedReaderSetting();
    }
}