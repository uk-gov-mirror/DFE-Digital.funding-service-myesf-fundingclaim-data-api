namespace Pds.FundingClaim.CorporateSchema.FundingClaims
{
    /// <summary>
    /// Represents the Deliverable Code.
    /// </summary>
    public class DeliverableCode
    {
        /// <summary>
        /// Gets or sets the value of the Deliverable Code.
        /// </summary>
        public int DeliverableCodeValue { get; set; }

        /// <summary>
        /// Gets or sets the actual volume of this deliverable.
        /// </summary>
        public int ActualVolume { get; set; }

        /// <summary>
        /// Gets or sets the actual value of this deliverable.
        /// </summary>
        public decimal ActualValue { get; set; }

        /// <summary>
        /// Gets or sets the forecast value of this deliverable.
        /// </summary>
        public decimal ForecastValue { get; set; }

        /// <summary>
        /// Gets or sets the adjustment value of this deliverable.
        /// </summary>
        public decimal AdjustmentValue { get; set; }

        /// <summary>
        /// Gets or sets the total delivery this deliverable.
        /// </summary>
        public decimal TotalDelivery { get; set; }
    }
}