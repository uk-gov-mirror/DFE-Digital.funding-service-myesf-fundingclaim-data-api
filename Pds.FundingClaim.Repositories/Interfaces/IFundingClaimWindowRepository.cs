using Pds.FundingClaim.Repositories.DataModels;
using System;

namespace Pds.FundingClaim.Repositories.Interfaces
{
    /// <summary>
    /// Repository to CRUD on FundingClaimWindow table in database.
    /// </summary>
    public interface IFundingClaimWindowRepository : IRepository<FundingClaimWindow>
    {
        /// <summary>
        /// Gets the last window that has passed signature close date.
        /// </summary>
        /// <param name="now">Current datetime to compare.</param>
        /// <returns>The last window details.</returns>
        FundingClaimWindow GetLastWindow(DateTime now);
    }
}