namespace Pds.FundingClaim.Repositories.Exceptions
{
    /// <summary>
    /// Exceptions to be raised if the Corporate Funding Claim's Ukprn is in the wrong format.
    /// </summary>
    public class UkprnWrongFormatException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UkprnWrongFormatException"/> class.
        /// Default constructor.
        /// </summary>
        /// <param name="ukprn">The Ukprn of the Corporate Funding Claim.</param>
        public UkprnWrongFormatException(string ukprn) : base($"The UKPRN {ukprn} contains non-numerical characters" +
            " or is blank.")
        {
        }
    }
}