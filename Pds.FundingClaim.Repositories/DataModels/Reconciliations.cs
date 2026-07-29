using Pds.FundingClaim.Repositories.Enums;
using System;

namespace Pds.FundingClaim.Repositories.DataModels
{
    /// <summary>
    /// Normalised version of a DC Reconciliation.
    /// </summary>
    public partial class Reconciliations
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Reconciliations"/> class.
        /// Default Constructor.
        /// </summary>
        public Reconciliations()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Reconciliations"/> class.
        /// Full Constructor.
        /// </summary>
        /// <param name="data">The original data from FCS.</param>
        /// <param name="ukprn">The UKPRN for whom the funding claim belongs.</param>
        /// <param name="version">The version of this instance.</param>
        /// <param name="type">The type of this instance.</param>
        /// <param name="period">The period to which this instance belongs.</param>
        /// <param name="title">The title of Reconciliation.</param>
        public Reconciliations(ReconciliationData data, int ukprn, int version, ReconciliationType type, string period, string title)
        {
            ReconciliationData = data;
            Ukprn = ukprn;
            Version = version;
            Type = type;
            Period = period;
            Title = title;

            data?.SetReconciliation(this);
        }

        #endregion

        public int Id { get; set; }

        /// <summary>
        /// Gets or sets who the reconciliation is for.
        /// </summary>
        public int Ukprn { get; set; }

        /// <summary>
        /// Gets or sets the friendly name of a reconciliation.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the version number of the reconciliation.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the type of reconciliation that this instance represents.
        /// </summary>
        public ReconciliationType Type { get; set; }

        /// <summary>
        /// Gets or sets the period for which this reconciliation belongs.
        /// </summary>
        public string Period { get; set; }

        /// <summary>
        /// Gets or sets when this instance was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets when this instance was last updated.
        /// </summary>
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets whether or not this reconciliation has passed validation.
        /// </summary>
        public bool? IsValid { get; set; }

        /// <summary>
        /// Gets or sets the original funding claim data for this reconciliation.
        /// </summary>
        public virtual ReconciliationData ReconciliationData { get; set; }
    }
}
