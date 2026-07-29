using Pds.FundingClaim.CorporateSchema.Reconciliations;
using System;

namespace Pds.FundingClaim.Services.Models
{
    /// <summary>
    /// Represents the syndicate feed item read from FCS atom feed.
    /// </summary>
    public class FeedReconciliation
    {
        /// <summary>
        /// Gets or sets the syndicate feed identifier.
        /// </summary>
        public Guid FeedId { get; set; }

        /// <summary>
        /// Gets or sets the corporate reconciliation object.
        /// </summary>
        public FCReconciliation Reconciliation { get; set; }
    }
}
