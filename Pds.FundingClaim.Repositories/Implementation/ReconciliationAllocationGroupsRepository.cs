using Pds.Core.Logging;
using Pds.FundingClaim.Repositories.Data;
using Pds.FundingClaim.Repositories.DataModels;

namespace Pds.FundingClaim.Repositories.Implementation
{
    /// <summary>
    /// Repository to CRUD on ReconciliationAllocationGroups table in database.
    /// </summary>
    public class ReconciliationAllocationGroupsRepository : Repository<ReconciliationAllocationGroups>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReconciliationAllocationGroupsRepository"/> class.
        /// The parametrised constructor.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">The logging service.</param>
        public ReconciliationAllocationGroupsRepository(
            PdsContext context, ILoggerAdapter<Repository<ReconciliationAllocationGroups>> logger) : base(context, logger)
        {
        }
    }
}