namespace Pds.FundingClaim.Repositories.Exceptions
{
    /// <summary>
    /// Exceptions to be raised if the Funding Claims Period has the wrong length of characters.
    /// </summary>
    public class PeriodWrongLengthException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PeriodWrongLengthException"/> class.
        /// Default constructor.
        /// </summary>
        /// <param name="period">The period of the Funding Claims.</param>
        public PeriodWrongLengthException(string period) : base($"The period {period}'s length is not 4 characters.")
        {
        }
    }
}