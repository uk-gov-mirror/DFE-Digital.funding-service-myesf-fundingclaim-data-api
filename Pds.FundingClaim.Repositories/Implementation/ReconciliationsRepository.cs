using Pds.Core.Logging;
using Pds.FundingClaim.Repositories.Data;
using Pds.FundingClaim.Repositories.DataModels;

namespace Pds.FundingClaim.Repositories.Implementation
{
    public class ReconciliationsRepository : Repository<Reconciliations>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReconciliationsRepository"/> class.
        /// The parametrised constructor.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">The logging service.</param>
        public ReconciliationsRepository(
            PdsContext context, ILoggerAdapter<Repository<Reconciliations>> logger) : base(context, logger)
        {
        }
    }
}