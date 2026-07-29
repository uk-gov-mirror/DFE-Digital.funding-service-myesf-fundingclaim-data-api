using Pds.FundingClaim.CorporateSchema.Reconciliations;

namespace Pds.FundingClaim.Repositories.DataModels
{
    /// <summary>
    /// Contains Original Data for the <see cref="Reconciliation"/>.
    /// </summary>
    public partial class ReconciliationData
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReconciliationData"/> class.
        /// Default Constructor.
        /// </summary>
        public ReconciliationData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReconciliationData"/> class.
        /// </summary>
        /// <param name="originalReconciliation">The original Reconciliation that contains the data.</param>
        public ReconciliationData(FCReconciliation originalReconciliation)
        {
            OriginalFundingClaimXml = originalReconciliation.ToXml().ToString();
        }

        #endregion

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Original Reconciliation as xml in the repository.
        /// </summary>
        public string OriginalFundingClaimXml { get; set; }

        /// <summary>
        /// Gets or sets the owning <see cref="Reconciliations"/>.
        /// </summary>
        public virtual Reconciliations IdNavigation { get; set; }

        /// <summary>
        /// Sets the owning Reconciliation on this instance.
        /// </summary>
        /// <param name="reconciliation">The parent Reconciliation.</param>
        /// <returns>This instance for method chaining.</returns>
        public ReconciliationData SetReconciliation(Reconciliations reconciliation)
        {
            IdNavigation = reconciliation;
            return this;
        }
    }
}
