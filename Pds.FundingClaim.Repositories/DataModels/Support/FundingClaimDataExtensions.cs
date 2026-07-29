namespace Pds.FundingClaim.Repositories.DataModels
{
    /// <summary>
    /// Extensions to <see cref="DataModels.FundingClaimData"/>.
    /// </summary>
    public static class FundingClaimDataExtensions
    {
        /// <summary>
        /// Sets the owning Funding Claims object on this instance.
        /// </summary>
        /// <param name="fundingClaimData">The FundingClaimDatas object whose funding claim will be set.</param>
        /// <param name="fundingClaim">The parent Funding Claims object.</param>
        /// <returns>This instance for method chaining.</returns>
        public static FundingClaimData SetFundingClaim(this FundingClaimData fundingClaimData, FundingClaim fundingClaim)
        {
            fundingClaimData.FundingClaim = fundingClaim;
            return fundingClaimData;
        }
    }
}