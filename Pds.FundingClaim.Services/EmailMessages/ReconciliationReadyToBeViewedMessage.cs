using Pds.FundingClaim.Services.EmailMessages;

namespace Sfa.Sfs.Contracts.Messaging
{
    /// <summary>
    /// Message for reconciliation that is ready to view.
    /// </summary>
    public class ReconciliationReadyToBeViewedMessage : IMessage
    {
        /// <summary>
        /// Gets or sets the id of the reconciliation that is ready to view.
        /// </summary>
        public int ReconciliationId { get; set; }
    }
}