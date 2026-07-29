using Pds.FundingClaim.Repositories.Enums;
using System;

namespace Pds.FundingClaim.Repositories.Exceptions
{
    /// <summary>
    /// Exceptions to be raised if the ClaimTypeName is invalid.
    /// </summary>
    public class ClaimTypeNameInvalidException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimTypeNameInvalidException"/> class.
        /// Default constructor.
        /// </summary>
        /// <param name="claimTypeName">The ClaimTypeName of the Corporate Funding Claim.</param>
        public ClaimTypeNameInvalidException(string claimTypeName) : base($"The ClaimTypeName {claimTypeName} is " +
            "invalid. Please use a ClaimTypeName that can be parsed to one of the FundingClaimTypes: " +
            string.Join(", ", Enum.GetNames(typeof(FundingClaimType))))
        {
        }
    }
}