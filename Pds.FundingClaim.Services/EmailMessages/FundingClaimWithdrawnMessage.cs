using Pds.FundingClaim.Services.EmailMessages;

namespace Sfa.Sfs.Contracts.Messaging
{
    /// <summary>
    /// Message for funding claim that has been withdrawn.
    /// </summary>
    public class FundingClaimWithdrawnMessage : IMessage
    {
        /// <summary>
        /// Gets or sets the id of the funding claim that has been withdrawn.
        /// </summary>
        public int FundingClaimId { get; set; }
    }
}