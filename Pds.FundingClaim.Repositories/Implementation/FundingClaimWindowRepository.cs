using Pds.Core.Logging;
using Pds.FundingClaim.Repositories.Data;
using Pds.FundingClaim.Repositories.DataModels;
using Pds.FundingClaim.Repositories.Interfaces;
using System;
using System.Linq;

namespace Pds.FundingClaim.Repositories.Implementation
{
    /// <inheritdoc cref="IFundingClaimWindowRepository"/>
    public class FundingClaimWindowRepository : Repository<FundingClaimWindow>, IFundingClaimWindowRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FundingClaimWindowRepository"/> class.
        /// The parametrised constructor.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="logger">The logging service.</param>
        public FundingClaimWindowRepository(
            PdsContext context,
            ILoggerAdapter<Repository<FundingClaimWindow>> logger)
            : base(context, logger)
        {
        }

        /// <inheritdoc/>
        public FundingClaimWindow GetLastWindow(DateTime now)
        {
            return Where(window =>
                       window.SignatureCloseDate.HasValue &&
                       window.SignatureCloseDate.Value <= now)
                .OrderByDescending(window => window.SignatureCloseDate)
                .FirstOrDefault();
        }
    }
}