using Pds.FundingClaim.Repositories.Enums;
using Pds.FundingClaim.Repositories.Enums.Support;
using Pds.FundingClaim.Repositories.Exceptions;

namespace Pds.FundingClaim.Repositories.DataModels.Support
{
    /// <summary>
    /// Extensions to <see cref="DataModels.FundingClaim"/>.
    /// </summary>
    public static class FundingClaimExtensions
    {
        /// <summary>
        /// Sets the Title of the <see cref="FundingClaim"/>.
        /// </summary>
        /// <param name="fundingClaim">The FundingClaim whose title will be set.</param>
        public static void SetTitle(this FundingClaim fundingClaim)
        {
            if (fundingClaim.Period.Length != 4)
            {
                throw new PeriodWrongLengthException(fundingClaim.Period);
            }

            var periodStart = fundingClaim.Period.Substring(0, 2);
            var periodEnd = fundingClaim.Period.Substring(2);
            int periodNumber;

            try
            {
                periodNumber = int.Parse(fundingClaim.Period);
            }
            catch
            {
                throw new PeriodWrongFormatException(fundingClaim.Period);
            }

            if (fundingClaim.Type == FundingClaimType.YEAREND && periodNumber >= 1819)
            {
                fundingClaim.Title = $"Year end (R10) funding claim for 20{periodStart} to 20{periodEnd} version " +
                    $"{fundingClaim.Version}";
            }
            else
            {
                fundingClaim.Title = $"{fundingClaim.Type.GetDisplayName()} funding claim for 20{periodStart} to " +
                    $"20{periodEnd} version {fundingClaim.Version}";
            }
        }
    }
}