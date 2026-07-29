using Pds.FundingClaim.Services.EmailMessages;

namespace Sfa.Sfs.Contracts.Messaging
{
    /// <summary>
    /// Message for funding claim that is ready to view.
    /// </summary>
    public class FundingClaimReadyToViewMessage : IMessage
    {
        /// <summary>
        /// Gets or sets the id of the funding claim that is ready to view.
        /// </summary>
        public int FundingClaimId { get; set; }
    }
}