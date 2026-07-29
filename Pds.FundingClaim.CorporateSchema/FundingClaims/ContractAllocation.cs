namespace Pds.FundingClaim.CorporateSchema.FundingClaims
{
    /// <summary>
    /// Represents the Contract Allocation.
    /// </summary>
    public class ContractAllocation
    {
        /// <summary>
        /// Gets or sets the Contract Allocation Number.
        /// </summary>
        public string ContractAllocationNumber { get; set; }

        /// <summary>
        /// Gets or sets the code for the Funding Stream Period of the allocation.
        /// </summary>
        public string FundingStreamPeriodCode { get; set; }

        /// <summary>
        /// Gets or sets the maximum contract value of the allocation.
        /// </summary>
        public decimal MaximumContractValue { get; set; }

        /// <summary>
        /// Gets or sets the deliverable codes of the allocation.
        /// </summary>
        public DeliverableCode[] DeliverableCodes { get; set; }
    }
}