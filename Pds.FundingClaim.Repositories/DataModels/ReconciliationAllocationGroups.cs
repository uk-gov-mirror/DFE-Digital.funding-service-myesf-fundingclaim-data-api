namespace Pds.FundingClaim.Repositories.DataModels
{
    /// <summary>
    /// Allocation group mapping used for reconciliation friendly name.
    /// </summary>
    public partial class ReconciliationAllocationGroups
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReconciliationAllocationGroups"/> class.
        /// Default Constructor.
        /// </summary>
        public ReconciliationAllocationGroups()
        {
        }

        /// <summary>
        ///  Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the code used across the SFA for this instance.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the description of the reconciliation allocation group.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets what the purpose of this reconciliation allocation group is for.
        /// </summary>
        public int UsageType { get; set; }
    }
}
