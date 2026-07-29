using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Logging;
using Pds.FundingClaim.Api.Controllers;
using Pds.FundingClaim.Repositories.DataModels;
using Pds.FundingClaim.Repositories.Enums;
using Pds.FundingClaim.Services.Interfaces;
using Pds.FundingClaim.Services.Models;
using Sfa.Sfs.Contracts.Messaging;
using System;
using System.Threading.Tasks;

namespace Pds.FundingClaim.Api.Tests.Unit
{
    [TestClass]
    public class ReconciliationControllerTests
    {
        [TestMethod, TestCategory("Unit")]
        public async Task GetReconciliationFeedBookmarkIdSetting_WhenRequested_CreatesLogAndReturnsReconciliationFeedBookmarkId()
        {
            //arrange
            var resultFromService = "Test Result";
            var log = $"GetReconciliationFeedBookmarkIdSetting returned setting value: {resultFromService}";

            var mockSettingDataService = new Mock<ISettingDataService>(MockBehavior.Strict);
            mockSettingDataService.Setup(service => service.GetReconciliationFeedBookmarkIdSetting())
                             .ReturnsAsync(resultFromService);
            var mockLogger = new Mock<ILoggerAdapter<ReconciliationController>>(MockBehavior.Strict);
            mockLogger.Setup(
                l => l.LogInformation(log)).Verifiable();

            var reconciliationController = new ReconciliationController(mockSettingDataService.Object, null, mockLogger.Object);

            //act
            var response = await reconciliationController.GetReconciliationFeedBookmarkIdSetting();

            //assert
            response.Should().BeEquivalentTo(resultFromService);
            mockLogger.Verify(
                l => l.LogInformation(log), Times.Once);
        }


        [TestMethod, TestCategory("Unit")]
        public async Task GetFeedReadWarningThresholdSetting_WhenRequested_CreatesLogAndReturnsFeedReadWarningThreshold()
        {
            //arrange
            var resultFromService = "Test Result";
            var log = $"GetFeedReadWarningThresholdSetting returned setting value: {resultFromService}";

            var mockSettingDataService = new Mock<ISettingDataService>(MockBehavior.Strict);
            mockSettingDataService.Setup(service => service.GetFeedReadWarningThresholdSetting())
                             .ReturnsAsync(resultFromService);
            var mockLogger = new Mock<ILoggerAdapter<ReconciliationController>>(MockBehavior.Strict);
            mockLogger.Setup(
                l => l.LogInformation(log)).Verifiable();

            var reconciliationController = new ReconciliationController(mockSettingDataService.Object, null, mockLogger.Object);

            //act
            var response = await reconciliationController.GetFeedReadWarningThresholdSetting();

            //assert
            response.Should().BeEquivalentTo(resultFromService);
            mockLogger.Verify(
                l => l.LogInformation(log), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetUseNewReconciliationsFeedReaderSetting_WhenRequested_CreatesLogAndReturnsSetting()
        {
            //arrange
            var resultFromService = "Test Result";
            var log = $"GetUseNewReconciliationsFeedReaderSetting returned setting value: {resultFromService}";

            var mockSettingDataService = new Mock<ISettingDataService>(MockBehavior.Strict);
            mockSettingDataService.Setup(method => method.GetUseNewReconciliationsFeedReaderSetting())
                             .ReturnsAsync(resultFromService);
            var mockLogger = new Mock<ILoggerAdapter<ReconciliationController>>(MockBehavior.Strict);
            mockLogger.Setup(
                l => l.LogInformation(log)).Verifiable();

            var reconciliationController = new ReconciliationController(mockSettingDataService.Object, null, mockLogger.Object);

            //act
            var response = await reconciliationController.GetUseNewReconciliationsFeedReaderSetting();

            //assert
            response.Should().BeEquivalentTo(resultFromService);
            mockLogger.Verify(
                l => l.LogInformation(log), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task CreateReconciliation_CreatesLogAndReturnsSuccessMessage()
        {
            //arrange
            var log = $"CreateReconciliation ran successfully.";
            var feedReconciliation = new FeedReconciliation();

            var mockReconciliationDataService = new Mock<IReconciliationDataService>(MockBehavior.Strict);
            mockReconciliationDataService.Setup(service => service.CreateReconciliation(feedReconciliation))
                 .Returns(Task.CompletedTask);

            var mockLogger = new Mock<ILoggerAdapter<ReconciliationController>>(MockBehavior.Strict);
            mockLogger.Setup(
                l => l.LogInformation(log)).Verifiable();

            var reconciliationController = new ReconciliationController(null, mockReconciliationDataService.Object, mockLogger.Object);

            //act
            var response = await reconciliationController.CreateReconciliation(feedReconciliation);

            //assert
            response.Should().BeOfType(typeof(CreatedResult));
            mockLogger.Verify(
                l => l.LogInformation(log), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task AuditReconciliationFeedReadException_AuditsMessageAndReturnsSuccessMessage()
        {
            //arrange
            var message = "Error message";
            var log = $"AuditReconciliationFeedReadException audited message: {message}.";

            var mockReconciliationDataService = new Mock<IReconciliationDataService>(MockBehavior.Strict);
            mockReconciliationDataService.Setup(service => service.AuditReconciliationFeedReadException(message))
                 .Returns(Task.CompletedTask);

            var mockLogger = new Mock<ILoggerAdapter<ReconciliationController>>(MockBehavior.Strict);
            mockLogger.Setup(
                l => l.LogInformation(log)).Verifiable();

            var reconciliationController = new ReconciliationController(null, mockReconciliationDataService.Object, mockLogger.Object);

            //act
            var response = await reconciliationController.AuditReconciliationFeedReadException(message);

            //assert
            response.Should().BeOfType(typeof(CreatedResult));

            mockLogger.Verify(
                l => l.LogInformation(log), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task SendFeedReadExceptionEmail_SendsEmail()
        {
            // Arrange
            var message = new FeedReadExceptionMessage { Url = "Url", Type = FeedReadExceptionMessage.ExceptionType.EmptyPageOnFeed, Bookmark = Guid.NewGuid() };
            var log = $"SendFeedReadExceptionEmail sent message for type: {message.Type}.";

            var mockReconciliationDataService = new Mock<IReconciliationDataService>(MockBehavior.Strict);
            mockReconciliationDataService.Setup(service => service.SendFeedReadExceptionEmail(message))
                 .Returns(Task.CompletedTask);

            var mockLogger = new Mock<ILoggerAdapter<ReconciliationController>>(MockBehavior.Strict);
            mockLogger.Setup(
                l => l.LogInformation(log)).Verifiable();

            var reconciliationController = new ReconciliationController(null, mockReconciliationDataService.Object, mockLogger.Object);

            //act
            var response = await reconciliationController.SendFeedReadExceptionEmail(message);

            //assert
            response.Should().BeOfType(typeof(OkResult));

            mockLogger.Verify(
                l => l.LogInformation(log), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task SendFeedExceededReadThresholdWarningEmail_SendsEmail()
        {
            // Arrange
            var message = new FeedReadThresholdExceededWarningMessage
            {
                Start = DateTime.Now.AddMinutes(-10),
                Now = DateTime.Now,
                BookmarkId = Guid.NewGuid(),
                LastPageUrl = "url"
            };
            var log = $"SendFeedExceededReadThresholdWarningEmail sent message for url: {message.LastPageUrl}.";

            var mockReconciliationDataService = new Mock<IReconciliationDataService>(MockBehavior.Strict);
            mockReconciliationDataService.Setup(service => service.SendFeedExceededReadThresholdWarningEmail(message))
                 .Returns(Task.CompletedTask);

            var mockLogger = new Mock<ILoggerAdapter<ReconciliationController>>(MockBehavior.Strict);
            mockLogger.Setup(
                l => l.LogInformation(log)).Verifiable();

            var reconciliationController = new ReconciliationController(null, mockReconciliationDataService.Object, mockLogger.Object);

            //act
            var response = await reconciliationController.SendFeedExceededReadThresholdWarningEmail(message);

            //assert
            response.Should().BeOfType(typeof(OkResult));

            mockLogger.Verify(
                l => l.LogInformation(log), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateReconciliationFeedBookmarkId_CreatesLogAndReturnsSuccessMessage()
        {
            //arrange
            var bookmarkId = Guid.NewGuid();
            var log = $"Updated the reconciliation feed bookmarkid: {bookmarkId}.";

            var mockReconciliationDataService = new Mock<IReconciliationDataService>(MockBehavior.Strict);
            mockReconciliationDataService.Setup(service => service.UpdateReconciliationFeedBookmarkId(bookmarkId))
                .Returns(Task.CompletedTask);

            var mockLogger = new Mock<ILoggerAdapter<ReconciliationController>>(MockBehavior.Strict);
            mockLogger.Setup(
                l => l.LogInformation(log)).Verifiable();

            var reconciliationController = new ReconciliationController(null, mockReconciliationDataService.Object, mockLogger.Object);

            //act
            var response = await reconciliationController.UpdateReconciliationFeedBookmarkId(bookmarkId);

            //assert
            response.Should().BeOfType(typeof(OkResult));

            mockLogger.Verify(
                l => l.LogInformation(log), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetReconciliationById_ReturnsReconciliationByRequestedId()
        {
            // Arrange
            var reconciliation = new Reconciliations
            {
                Id = 1,
                Ukprn = 12345678,
                Title = "Reconciliation title",
                Version = 1,
                Type = ReconciliationType.FINAL,
                Period = "2425",
                CreatedAt = DateTime.Now,
                LastUpdatedAt = DateTime.Now,
            };

            var mockReconciliationDataService = new Mock<IReconciliationDataService>(MockBehavior.Strict);
            mockReconciliationDataService.Setup(service => service.GetReconciliationById(reconciliation.Id))
                 .Returns(Task.FromResult(reconciliation));

            var mockLogger = new Mock<ILoggerAdapter<ReconciliationController>>();

            var reconciliationController = new ReconciliationController(null, mockReconciliationDataService.Object, mockLogger.Object);

            //act
            var response = await reconciliationController.GetReconciliationById(reconciliation.Id);

            //assert
            response.Should().BeEquivalentTo(reconciliation);

            mockLogger.Verify(
                l => l.LogInformation($"Reconciliation with Id [{response.Id}] returned from GetReconciliationById using Id {reconciliation.Id}"), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetReconciliationById_ReturnsNullResponse()
        {
            // Arrange
            var mockReconciliationDataService = new Mock<IReconciliationDataService>(MockBehavior.Strict);
            mockReconciliationDataService.Setup(service => service.GetReconciliationById(1))
                 .Returns(Task.FromResult((Reconciliations)null));

            var mockLogger = new Mock<ILoggerAdapter<ReconciliationController>>();

            var reconciliationController = new ReconciliationController(null, mockReconciliationDataService.Object, mockLogger.Object);

            //act
            var response = await reconciliationController.GetReconciliationById(1);

            //assert
            response.Should().BeEquivalentTo((Reconciliations)null);

            mockLogger.Verify(
                l => l.LogInformation($"Null returned from GetReconciliationById using Id 1"), Times.Once);
        }
    }
}