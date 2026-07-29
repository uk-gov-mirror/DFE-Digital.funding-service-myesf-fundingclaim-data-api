using Pds.FundingClaim.CorporateSchema.FundingClaims;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServicesWindow = Pds.FundingClaim.Services.Models.FundingClaimWindow;

namespace Pds.FundingClaim.Services.Interfaces
{
    /// <summary>
    /// Interface for Funding Claim window related data operations.
    /// </summary>
    public interface IFundingClaimWindowDataService
    {
        #region Public Methods

        /// <summary>
        /// Gets the Funding Claim Current Window.
        /// </summary>
        /// <returns>The Funding Claim Current Window details.</returns>
        Task<ServicesWindow> GetFundingClaimCurrentWindow();

        /// <summary>
        /// Creates or updates the Funding Claim Current Window.
        /// </summary>
        /// <param name="fundingClaimDetails">List of funding claim windows to be added or updated.</param>
        /// <returns>The completed Task.</returns>
        Task UpdateFundingClaimWindow(IEnumerable<FundingClaimDetails> fundingClaimDetails);

        #endregion
    }
}