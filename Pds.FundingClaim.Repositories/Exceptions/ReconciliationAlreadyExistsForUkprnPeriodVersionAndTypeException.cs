using Pds.FundingClaim.Repositories.DataModels;
using System;

namespace Pds.FundingClaim.Repositories.Exceptions
{
    /// <summary>
    /// Exceptions to be raised if matching reconciliation already exists in database.
    /// </summary>
    public class ReconciliationAlreadyExistsForUkprnPeriodVersionAndTypeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReconciliationAlreadyExistsForUkprnPeriodVersionAndTypeException"/> class.
        /// Default constructor.
        /// </summary>
        /// <param name="ukprn">The Ukprn of the Corporate Funding Claim.</param>
        public ReconciliationAlreadyExistsForUkprnPeriodVersionAndTypeException(Reconciliations reconciliation) : base($"A reconciliation with UKPRN: {reconciliation.Ukprn}, Period: {reconciliation.Period}, Version: {reconciliation.Version}, Title: {reconciliation.Title} and Type: {reconciliation.Type} already exists.")
        {
        }
    }
}
