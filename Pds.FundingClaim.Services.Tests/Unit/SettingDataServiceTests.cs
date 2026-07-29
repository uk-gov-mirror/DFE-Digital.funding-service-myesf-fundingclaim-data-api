using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.FundingClaim.Repositories.DataModels;
using Pds.FundingClaim.Repositories.Interfaces;
using Pds.FundingClaim.Services.Constants;
using Pds.FundingClaim.Services.Implementations;
using System.Threading.Tasks;

namespace Pds.FundingClaim.Services.Tests.Unit
{
    [TestClass]
    public class SettingDataServiceTests
    {
        [TestMethod, TestCategory("Unit")]
        public async Task GetFundingClaimLastRetrievedSetting_WhenCalled_ReturnsFundingClaimLastRetrievedSetting()
        {
            //Arrange
            var resultFromRepository = new Setting { Value = "Test Result" };
            var mockSettingRepository = new Mock<IRepository<Setting>>();
            mockSettingRepository.Setup(service => service.FirstOrDefault(setting => setting.Type == ServiceConstants.FundingClaimLastRetrievedSetting))
                                  .ReturnsAsync(resultFromRepository);

            var settingDataService = new SettingDataService(mockSettingRepository.Object);

            //Act
            var response = await settingDataService.GetFundingClaimLastRetrievedSetting();

            //assert
            response.Should().BeEquivalentTo(resultFromRepository.Value);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetFundingClaimPollingSetting_WhenCalled_ReturnsFundingClaimPollingSetting()
        {
            //Arrange
            var resultFromRepository = new Setting { Value = "Test Result" };
            var mockSettingRepository = new Mock<IRepository<Setting>>();
            mockSettingRepository.Setup(service => service.FirstOrDefault(setting => setting.Type == ServiceConstants.FundingClaimPollingSetting))
                                  .ReturnsAsync(resultFromRepository);

            var settingDataService = new SettingDataService(mockSettingRepository.Object);

            //Act
            var response = await settingDataService.GetFundingClaimPollingSetting();

            //Assert
            response.Should().BeEquivalentTo(resultFromRepository.Value);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetReconciliationFeedBookmarkIdSetting_WhenCalled_ReturnsReconciliationFeedBookmarkIdSetting()
        {
            //Arrange
            var resultFromRepository = new Setting { Value = "Test Result" };
            var mockSettingRepository = new Mock<IRepository<Setting>>();
            mockSettingRepository.Setup(service =>
                                        service.FirstOrDefault(setting => setting.Type == ServiceConstants.ReconciliationFeedBookmarkIdSetting))
                                 .ReturnsAsync(resultFromRepository);

            var settingDataService = new SettingDataService(mockSettingRepository.Object);

            //Act
            var response = await settingDataService.GetReconciliationFeedBookmarkIdSetting();

            //Assert
            response.Should().BeEquivalentTo(resultFromRepository.Value);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetFeedReadWarningThresholdSetting_WhenCalled_ReturnsFeedReadWarningThresholdSetting()
        {
            //Arrange
            var resultFromRepository = new Setting { Value = "Test Result" };
            var mockSettingRepository = new Mock<IRepository<Setting>>(MockBehavior.Strict);
            mockSettingRepository.Setup(service =>
                                        service.FirstOrDefault(setting => setting.Type == ServiceConstants.FeedReadWarningThreshold))
                                 .ReturnsAsync(resultFromRepository);

            var settingDataService = new SettingDataService(mockSettingRepository.Object);

            //Act
            var response = await settingDataService.GetFeedReadWarningThresholdSetting();

            //Assert
            response.Should().BeEquivalentTo(resultFromRepository.Value);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetUseJsonFormatOfFundingClaimsSetting_WhenCalled_ReturnsSetting()
        {
            //Arrange
            var resultFromRepository = new Setting { Value = "Test Result" };
            var mockSettingRepository = new Mock<IRepository<Setting>>();
            mockSettingRepository.Setup(method =>
                                        method.FirstOrDefault(setting => setting.Type == ServiceConstants.UseJsonFormatOfFundingClaims))
                                 .ReturnsAsync(resultFromRepository);

            var settingDataService = new SettingDataService(mockSettingRepository.Object);

            //Act
            var response = await settingDataService.GetUseJsonFormatOfFundingClaimsSetting();

            //Assert
            response.Should().BeEquivalentTo(resultFromRepository.Value);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetUseNewReconciliationsFeedReaderSetting_WhenCalled_ReturnsSetting()
        {
            //Arrange
            var resultFromRepository = new Setting { Value = "Test Result" };
            var mockSettingRepository = new Mock<IRepository<Setting>>();
            mockSettingRepository.Setup(method =>
                                        method.FirstOrDefault(setting => setting.Type == ServiceConstants.UseNewReconciliationsFeedReader))
                                 .ReturnsAsync(resultFromRepository);

            var settingDataService = new SettingDataService(mockSettingRepository.Object);

            //Act
            var response = await settingDataService.GetUseNewReconciliationsFeedReaderSetting();

            //Assert
            response.Should().BeEquivalentTo(resultFromRepository.Value);
        }
    }
}