using System.ComponentModel.DataAnnotations;

namespace Pds.FundingClaim.Repositories.Enums
{
    /// <summary>
    /// Defines the type that a funding claim can be.
    /// </summary>
    public enum FundingClaimType
    {
        /// <summary>
        /// The Funding claim is a mid-year funding claim.
        /// </summary>
        [Display(Name = "Mid year (R06)")]
        MIDYEAR = 0,

        /// <summary>
        /// The Funding claim is a year-end funding claim.
        /// </summary>
        [Display(Name = "Year end (R13)")]
        YEAREND = 1,

        /// <summary>
        /// The Funding claim is a final funding claim.
        /// </summary>
        [Display(Name = "Final (R14)")]
        FINAL = 2
    }
}