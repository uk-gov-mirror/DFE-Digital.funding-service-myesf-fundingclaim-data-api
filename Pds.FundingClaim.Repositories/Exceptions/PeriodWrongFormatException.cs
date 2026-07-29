namespace Pds.FundingClaim.Repositories.Exceptions
{
    /// <summary>
    /// Exceptions to be raised if the Funding Claims Period is in the wrong format.
    /// </summary>
    public class PeriodWrongFormatException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PeriodWrongFormatException"/> class.
        /// Default constructor.
        /// </summary>
        /// <param name="period">The period of the Funding Claims.</param>
        public PeriodWrongFormatException(string period) : base($"The period {period} is in the wrong format. " +
            "It should have the last two numbers of the starting year followed by the last two numbers of the " +
            "ending year. Example: '1920'.")
        {
        }
    }
}