using Pds.FundingClaim.Repositories.DataModels;
using Pds.FundingClaim.Repositories.Interfaces;
using Pds.FundingClaim.Services.Constants;
using Pds.FundingClaim.Services.Interfaces;
using System.Threading.Tasks;

namespace Pds.FundingClaim.Services.Implementations
{
    /// <inheritdoc cref="ISettingDataService"/>
    public class SettingDataService : ISettingDataService
    {
        #region Private Members

        private IRepository<Setting> _settingsRepository;

        #endregion


        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingDataService"/> class.
        /// The parameterised constructor.
        /// </summary>
        /// <param name="settingsRepository">The setting repository service.</param>
        public SettingDataService(
            IRepository<Setting> settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        #endregion


        #region Implementation

        /// <inheritdoc/>
        public async Task<string> GetFundingClaimLastRetrievedSetting()
        {
            var result = await _settingsRepository.FirstOrDefault(setting => setting.Type == ServiceConstants.FundingClaimLastRetrievedSetting);

            return result?.Value;
        }

        /// <inheritdoc/>
        public async Task<string> GetFundingClaimPollingSetting()
        {
            var result = await _settingsRepository.FirstOrDefault(setting => setting.Type == ServiceConstants.FundingClaimPollingSetting);

            return result?.Value;
        }

        /// <inheritdoc/>
        public async Task<string> GetReconciliationFeedBookmarkIdSetting()
        {
            var result = await _settingsRepository.FirstOrDefault(setting => setting.Type == ServiceConstants.ReconciliationFeedBookmarkIdSetting);

            return result?.Value;
        }

        /// <inheritdoc/>
        public async Task<string> GetFeedReadWarningThresholdSetting()
        {
            var result = await _settingsRepository.FirstOrDefault(setting => setting.Type == ServiceConstants.FeedReadWarningThreshold);

            return result?.Value;
        }

        /// <inheritdoc/>
        public async Task<string> GetUseJsonFormatOfFundingClaimsSetting()
        {
            var result = await _settingsRepository.FirstOrDefault(setting => setting.Type == ServiceConstants.UseJsonFormatOfFundingClaims);

            return result?.Value;
        }

        /// <inheritdoc/>
        public async Task<string> GetUseNewReconciliationsFeedReaderSetting()
        {
            var result = await _settingsRepository.FirstOrDefault(setting => setting.Type == ServiceConstants.UseNewReconciliationsFeedReader);

            return result?.Value;
        }

        #endregion
    }
}