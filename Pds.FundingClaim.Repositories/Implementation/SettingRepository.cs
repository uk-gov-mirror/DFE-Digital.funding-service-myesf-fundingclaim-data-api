using Pds.Core.Logging;
using Pds.FundingClaim.Repositories.Data;
using Pds.FundingClaim.Repositories.DataModels;

namespace Pds.FundingClaim.Repositories.Implementation
{
    /// <summary>
    /// Repository to CRUD on Settings table in database.
    /// </summary>
    public class SettingRepository : Repository<Setting>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingRepository"/> class.
        /// The parametrised constructor.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">The logging service.</param>
        public SettingRepository(PdsContext context, ILoggerAdapter<Repository<Setting>> logger) : base(context, logger)
        {
        }
    }
}