using Pds.FundingClaim.Repositories.Enums;
using System;

namespace Pds.FundingClaim.Services.Models
{
    /// <summary>
    /// Represents a funding claim.
    /// </summary>
    public class FundingClaim
    {
        /// <summary>
        /// Gets or sets identifier for the funding claim.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets who the funding claim is for.
        /// </summary>
        public int Ukprn { get; set; }

        /// <summary>
        /// Gets or sets the friendly name of a funding claim.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the version number of the funding claim.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the type of funding claim that this instance represents.
        /// </summary>
        public FundingClaimType Type { get; set; }

        /// <summary>
        /// Gets or sets the period for which this funding claim belongs.
        /// </summary>
        public string Period { get; set; }

        /// <summary>
        /// Gets or sets if a funding claim has been signed, then this will be populated by the name of the provider agent who signed it.
        /// </summary>
        public string SignedBy { get; set; }

        /// <summary>
        /// Gets or sets if a funding claim has been signed, then this will be populated by the display name of the provider agent who signed it.
        /// </summary>
        public string SignedByDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the date and time that the contract was signed.
        /// </summary>
        public DateTime? SignedOn { get; set; }

        /// <summary>
        /// Gets or sets when this instance was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets when this instance was last updated.
        /// </summary>
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets unique identifier of funding claim.
        /// </summary>
        public string FundingClaimUniqueId { get; set; }

        /// <summary>
        /// Gets or sets related funding claim window.
        /// </summary>
        public int FundingClaimWindowId { get; set; }

        /// <summary>
        /// Gets or sets the Status of the funding claim.
        /// </summary>
        public FundingClaimState Status { get; set; }

        /// <summary>
        /// Gets or sets the time at which a funding claim has been submitted.
        /// </summary>
        public DateTime? DateSubmitted { get; set; }

        /// <summary>
        /// Gets or sets related funding claim window.
        /// </summary>
        public FundingClaimWindow FundingClaimWindow { get; set; }
    }
}
