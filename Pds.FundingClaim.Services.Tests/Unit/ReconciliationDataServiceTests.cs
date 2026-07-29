using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Audit.Api.Client.Interfaces;
using Pds.Core.Logging;
using Pds.FundingClaim.CorporateSchema.Reconciliations;
using Pds.FundingClaim.Repositories.DataModels;
using Pds.FundingClaim.Repositories.Enums;
using Pds.FundingClaim.Repositories.Enums.Support;
using Pds.FundingClaim.Repositories.Interfaces;
using Pds.FundingClaim.Services.Constants;
using Pds.FundingClaim.Services.Implementations;
using Pds.FundingClaim.Services.Interfaces;
using Pds.FundingClaim.Services.Models;
using Sfa.Sfs.Contracts.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AuditModels = Pds.Audit.Api.Client.Models;

namespace Pds.FundingClaim.Services.Tests.Unit
{
    [TestClass]
    public class ReconciliationDataServiceTests
    {
        private Mock<IRepository<ReconciliationAllocationGroups>> _mockReconciliationAllocationGroupsRepository;
        private Mock<IRepository<Reconciliations>> _mockReconciliationRepository;
        private Mock<IRepository<Setting>> _mockSettingRepository;
        private Mock<IAuditService> _mockAuditService;
        private Mock<ISystemProvider> _mockSystemProvider;
        private Mock<ILoggerAdapter<ReconciliationDataService>> _loggerAdapter;
        private Mock<IEmailService> _mockEmailService;

        [TestMethod, TestCategory("Unit")]
        public async Task CreateReconciliation_WhenMatchingReconciliationExists_AuditsError()
        {
            // Arrange
            var existingReconciliation = GetFeedReconciliation(12345678);
            var feedReconciliation = new FeedReconciliation();
            SetUpTests(existingReconciliation, true);

            var service = new ReconciliationDataService(_mockReconciliationRepository.Object, _mockReconciliationAllocationGroupsRepository.Object, _mockSettingRepository.Object, null, _mockSystemProvider.Object, _mockEmailService.Object, _mockAuditService.Object);

            // Act
            await service.CreateReconciliation(existingReconciliation);

            // Assert
            _mockAuditService.Verify(
             repo => repo.AuditAsync(It.IsAny<AuditModels.Audit>()), Times.Exactly(1));

            _mockReconciliationRepository.Verify(
                repo => repo.Create(It.IsAny<Reconciliations>()),
                Times.Never);

            _mockEmailService.Verify(
                es => es.SendReconciliationReadyToBeViewedEmail(It.IsAny<int>()),
                Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task CreateReconciliations_WhenUkprnIsZero_LogsError()
        {
            // Arrange
            var reconciliationWithUkprnAs0 = GetFeedReconciliation(0);

            SetUpTests(reconciliationWithUkprnAs0, false);

            var service = new ReconciliationDataService(_mockReconciliationRepository.Object, _mockReconciliationAllocationGroupsRepository.Object, _mockSettingRepository.Object, _loggerAdapter.Object, _mockSystemProvider.Object, _mockEmailService.Object, null);

            // Act
            Func<Task> act = () => service.CreateReconciliation(reconciliationWithUkprnAs0);
            await act.Should().ThrowAsync<InvalidDataException>();

            // Assert
            _loggerAdapter.Verify(
                logger => logger.LogError($"UKPRN is not found for OrgId1."),
                Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task CreateReconciliations_WhenValidReconciliation_CreatesInRepository()
        {
            // Arrange
            var reconciliationOne = GetFeedReconciliation(12345678);

            SetUpTests(reconciliationOne, false);

            var service = new ReconciliationDataService(_mockReconciliationRepository.Object, _mockReconciliationAllocationGroupsRepository.Object, _mockSettingRepository.Object, _loggerAdapter.Object, _mockSystemProvider.Object, _mockEmailService.Object, _mockAuditService.Object);

            // Act
            await service.CreateReconciliation(reconciliationOne);

            // Assert
            _mockReconciliationRepository.Verify(
                repo => repo.Create(It.Is<Reconciliations>(recon =>
                string.Equals(recon.ReconciliationData.OriginalFundingClaimXml, reconciliationOne.Reconciliation.ToXml().ToString()) &&
                recon.Ukprn == 12345678 &&
                recon.Version == 1 &&
                recon.Type == ReconciliationType.YEAREND &&
                string.Equals(recon.Period, "1516") &&
                string.Equals(recon.Title, "Year end reconciliation for Description for 2015 to 2016"))),
                Times.Once);

            _mockEmailService.Verify(
                es => es.SendReconciliationReadyToBeViewedEmail(1),
                Times.Once);

            _mockAuditService.Verify(
             repo => repo.AuditAsync(It.IsAny<AuditModels.Audit>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateReconciliationFeedBookmarkId_WhenCalled_UpdateBookmarkID()
        {
            // Arrange
            var bookmarkId = Guid.NewGuid();
            var resultFromRepository = new Setting { Value = "Test Result" };
            _mockSettingRepository = new Mock<IRepository<Setting>>();
            _mockSettingRepository.Setup(method => method.FirstOrDefault(setting => setting.Type == ServiceConstants.ReconciliationFeedBookmarkIdSetting))
                .ReturnsAsync(resultFromRepository);

            var service = new ReconciliationDataService(null, null, _mockSettingRepository.Object, null, null, null, null);

            // Act
            await service.UpdateReconciliationFeedBookmarkId(bookmarkId);

            // Assert
            _mockSettingRepository.Verify(
                repo => repo.Update(
                    It.Is<Setting>(fw => fw.Value == bookmarkId.ToString())),
                Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task AuditReconciliationFeedReadException_WhenCalled_AuditsMessage()
        {
            // Arrange
            var message = "Error message";

            MockAuditService();
            var service = new ReconciliationDataService(null, null, null, null, null, null, _mockAuditService.Object);

            // Act
            await service.AuditReconciliationFeedReadException(message);

            // Assert
            _mockAuditService.Verify(
               repo => repo.AuditAsync(It.IsAny<AuditModels.Audit>()), Times.Exactly(1));
        }

        [TestMethod, TestCategory("Unit")]
        public async Task SendFeedReadExceptionEmail_WhenCalled_EmailsMessage()
        {
            // Arrange
            var message = new FeedReadExceptionMessage { Url = "Url", Type = FeedReadExceptionMessage.ExceptionType.EmptyPageOnFeed, Bookmark = Guid.NewGuid() };

            _mockEmailService = new Mock<IEmailService>(MockBehavior.Strict);
            _mockEmailService.Setup(service => service.SendFeedReadExceptionEmail(message)).Returns(Task.CompletedTask);

            var service = new ReconciliationDataService(null, null, null, null, null, _mockEmailService.Object, null);

            // Act
            await service.SendFeedReadExceptionEmail(message);

            // Assert
            _mockEmailService.Verify(
                service => service.SendFeedReadExceptionEmail(
                    It.Is<FeedReadExceptionMessage>(
                    m =>
                        m.Url == message.Url &&
                        m.Bookmark == message.Bookmark &&
                        m.Type == message.Type)),
                Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task SendFeedExceededReadThresholdWarningEmail_WhenCalled_EmailsMessageAndAudits()
        {
            // Arrange
            var message = new FeedReadThresholdExceededWarningMessage
            {
                Start = DateTime.Now.AddMinutes(-10),
                Now = DateTime.Now,
                BookmarkId = Guid.NewGuid(),
                LastPageUrl = "url"
            };

            var thresholdReachedMessage = $"Reconciliation Feed read threshold exceeded whilst looking for {message.BookmarkId}. Read started at {message.Start}, warning raised at {message.Now}. Last page read was {message.LastPageUrl}.";

            _mockEmailService = new Mock<IEmailService>(MockBehavior.Strict);
            _mockEmailService.Setup(service => service.SendFeedExceededReadThresholdWarningEmail(message)).Returns(Task.CompletedTask);

            MockAuditService();

            var service = new ReconciliationDataService(null, null, null, null, null, _mockEmailService.Object, _mockAuditService.Object);

            // Act
            await service.SendFeedExceededReadThresholdWarningEmail(message);

            // Assert
            _mockEmailService.Verify(
                service => service.SendFeedExceededReadThresholdWarningEmail(
                    It.Is<FeedReadThresholdExceededWarningMessage>(
                    m =>
                        m.Start == message.Start &&
                        m.Now == message.Now &&
                        m.BookmarkId == message.BookmarkId &&
                        m.LastPageUrl == message.LastPageUrl)),
                Times.Once);


            _mockAuditService.Verify(
               repo => repo.AuditAsync(It.IsAny<AuditModels.Audit>()), Times.Exactly(1));
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetReconciliationById_WhenCalledWithValidId_ReturnsReconciliationByRequestedId()
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

            _mockReconciliationRepository = new Mock<IRepository<Reconciliations>>();
            _mockReconciliationRepository.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<Reconciliations, bool>>>()))
                .Returns(Task.FromResult(reconciliation));

            var service = new ReconciliationDataService(_mockReconciliationRepository.Object, null, null, null, null, null, null);

            // Act
            var result = await service.GetReconciliationById(reconciliation.Id);

            // Assert
            result.Should().BeEquivalentTo(reconciliation);

            _mockReconciliationRepository.Verify(
                service => service.FirstOrDefault(It.IsAny<Expression<Func<Reconciliations, bool>>>()),
                Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetReconciliationById_WhenCalledWithNoMatchedId_ReturnsNull()
        {
            // Arrange
            _mockReconciliationRepository = new Mock<IRepository<Reconciliations>>();
            _mockReconciliationRepository.Setup(x => x.FirstOrDefault(It.IsAny<Expression<Func<Reconciliations, bool>>>()))
                .Returns(Task.FromResult<Reconciliations>(null));

            var service = new ReconciliationDataService(_mockReconciliationRepository.Object, null, null, null, null, null, null);

            // Act
            var result = await service.GetReconciliationById(It.IsAny<int>());

            // Assert
            result.Should().BeNull();

            _mockReconciliationRepository.Verify(
                service => service.FirstOrDefault(It.IsAny<Expression<Func<Reconciliations, bool>>>()),
                Times.Once);
        }

        #region Private Helpers

        private FeedReconciliation GetFeedReconciliation(int ukprn)
        {
            return new FeedReconciliation { Reconciliation = GetCorporateReconciliation(ukprn), FeedId = Guid.NewGuid() };
        }

        private FCReconciliation GetCorporateReconciliation(int ukprn)
        {
            return new FCReconciliation
            {
                SchemaVersion = 1,
                FCReconciliationAllAttrs = new FCReconciliationAllAttrs
                {
                    FCReconciliationPKeyAttrs = new FCReconciliationPKeyAttrs
                    {
                        Contractor = new Contractor
                        {
                            ContractorPKeyAttrs = new ContractorPKeyAttrs
                            {
                                OrganisationIdentifier = "OrgId1"
                            },
                            ContractorNonPKeyAttrs = new ContractorNonPKeyAttrs
                            {
                                UKPRN = ukprn
                            }
                        },
                        Period = new Period
                        {
                            PeriodValue = "1516",
                            PeriodTypePKeyAttrs = new PeriodTypePKeyAttrs
                            {
                                PeriodTypeCode = PeriodTypeCode.AY
                            }
                        },
                        ClaimVersionNumber = 1,
                        ClaimType = new ClaimType
                        {
                            ClaimTypePKeyAttrs = new ClaimTypePKeyAttrs
                            {
                                ClaimTypeCode = "Year End"
                            },
                            ClaimTypeNonPKeyAttrs = new ClaimTypeNonPKeyAttrs
                            {
                                ClaimTypeName = "Year End"
                            }
                        },
                        AllocationGroup = new AllocationGroup
                        {
                            AllocationGroupPKeyAttrs = new AllocationGroupPKeyAttrs
                            {
                                AllocationGroupCode = "ASC1516"
                            },
                            AllocationGroupNonPKeyAttrs = new AllocationGroupNonPKeyAttrs
                            {
                                AllocationGroupName = "ASC1516"
                            }
                        }
                    },
                    FCReconciliationNonPKeyAttrs = new FCReconciliationNonPKeyAttrs
                    {
                        Status = ReconciliationStatus.Published,
                        StatusLastUpdatedDate = DateTime.Now,
                        AmountAttrs = new AmountAttrs
                        {
                            PlannedValue = 1900,
                            PlannedValueSpecified = true,
                            ClaimedValue = 2000,
                            ClaimedValueSpecified = true,
                            ProposedReconciliationValue = 100,
                            ProposedReconciliationValueSpecified = true,
                            ReconciliationValue = 100,
                            ReconciliationValueSpecified = true,
                            CappedClaimedValue = 200,
                            CappedClaimedValueSpecified = true
                        },
                        ContractAllocation = new[]
                         {
                            new ContractAllocation
                            {
                                ContractAllocationPKeyAttrs = new ContractAllocationPKeyAttrs()
                                {
                                    ContractAllocationNumber = "CA1"
                                },
                                ContractAllocationNonPKeyAttrs = new ContractAllocationNonPKeyAttrs()
                                {
                                    FundingstreamPeriodCode = "ASC1516",
                                    ContractDeliverables = new[]
                                    {
                                        new ContractDeliverable
                                        {
                                            Deliverable = new Deliverable
                                            {
                                                DeliverableCode = 1
                                            },
                                            AmountAttrs = new AmountAttrs
                                            {
                                                PlannedValue = 3000,
                                                PlannedValueSpecified = true,
                                                ClaimedValue = 2000,
                                                ClaimedValueSpecified = true,
                                                ProposedReconciliationValue = -1000,
                                                ProposedReconciliationValueSpecified = true,
                                                ReconciliationValue = -1000,
                                                ReconciliationValueSpecified = true,
                                                CappedClaimedValue = 2000,
                                                CappedClaimedValueSpecified = true
                                            }
                                        },
                                        new ContractDeliverable
                                        {
                                            Deliverable = new Deliverable
                                            {
                                                DeliverableCode = 2
                                            },
                                            AmountAttrs = new AmountAttrs
                                            {
                                                PlannedValue = 1900,
                                                PlannedValueSpecified = true,
                                                ClaimedValue = 2900,
                                                ClaimedValueSpecified = true,
                                                ProposedReconciliationValue = 1000,
                                                ProposedReconciliationValueSpecified = true,
                                                ReconciliationValue = 1000,
                                                ReconciliationValueSpecified = true,
                                                CappedClaimedValue = 2000,
                                                CappedClaimedValueSpecified = true
                                            }
                                        },
                                        new ContractDeliverable
                                        {
                                            Deliverable = new Deliverable
                                            {
                                                DeliverableCode = 3
                                            },
                                            AmountAttrs = new AmountAttrs
                                            {
                                                PlannedValue = 2000,
                                                PlannedValueSpecified = true,
                                                ClaimedValue = 3000,
                                                ClaimedValueSpecified = true,
                                                ProposedReconciliationValue = 1000,
                                                ProposedReconciliationValueSpecified = true,
                                                ReconciliationValue = 1000,
                                                ReconciliationValueSpecified = true,
                                                CappedClaimedValue = 2000,
                                                CappedClaimedValueSpecified = true
                                            }
                                        },
                                        new ContractDeliverable
                                        {
                                            Deliverable = new Deliverable
                                            {
                                                DeliverableCode = 4
                                            },
                                            AmountAttrs = new AmountAttrs
                                            {
                                                PlannedValue = 0,
                                                PlannedValueSpecified = true,
                                                ClaimedValue = 0,
                                                ClaimedValueSpecified = true,
                                                ProposedReconciliationValue = 0,
                                                ProposedReconciliationValueSpecified = true,
                                                ReconciliationValue = 0,
                                                ReconciliationValueSpecified = true,
                                                CappedClaimedValue = 0,
                                                CappedClaimedValueSpecified = true
                                            }
                                        }
                                    }
                                }
                            }
                         }
                    }
                }
            };
        }

        private void SetUpTests(FeedReconciliation reconciliation, bool existingReconciliation)
        {
            _mockReconciliationAllocationGroupsRepository = new Mock<IRepository<ReconciliationAllocationGroups>>();
            _mockReconciliationAllocationGroupsRepository.Setup(method => method.Where(It.IsAny<Expression<Func<ReconciliationAllocationGroups,
                                                bool>>>()))
                                  .Returns(new List<ReconciliationAllocationGroups> { new ReconciliationAllocationGroups { Description = "Description" } });

            _mockReconciliationRepository = new Mock<IRepository<Reconciliations>>();
            if (existingReconciliation)
            {
                _mockReconciliationRepository.Setup(method => method.Where(It.IsAny<Expression<Func<Reconciliations,
                                                    bool>>>()))
                                      .Returns(new List<Reconciliations> { new Reconciliations(new ReconciliationData(reconciliation.Reconciliation), 1, 1, ReconciliationType.FINAL, "period", "title") });
            }

            SetUpMockReconciliationRepositoryCreateMethod(reconciliation);

            var resultFromRepository = new Setting { Value = "Test Result" };
            _mockSettingRepository = new Mock<IRepository<Setting>>();
            _mockSettingRepository.Setup(method => method.FirstOrDefault(setting => setting.Type == ServiceConstants.ReconciliationFeedBookmarkIdSetting))
                .ReturnsAsync(resultFromRepository);

            MockAuditService();

            _loggerAdapter = new Mock<ILoggerAdapter<ReconciliationDataService>>();

            _mockSystemProvider = new Mock<ISystemProvider>();
            _mockSystemProvider.Setup(method => method.UtcNow())
                                  .Returns(new DateTime(2000, 1, 1));

            _mockEmailService = new Mock<IEmailService>();
        }

        private void MockAuditService()
        {
            _mockAuditService = new Mock<IAuditService>();

            _mockAuditService
               .Setup(e => e.AuditAsync(It.IsAny<AuditModels.Audit>()))
               .Returns(Task.CompletedTask);
        }

        private void SetUpMockReconciliationRepositoryCreateMethod(FeedReconciliation reconciliation)
        {
            var fCReconciliation = reconciliation.Reconciliation;
            var reconciliationAttributes = fCReconciliation.FCReconciliationAllAttrs.FCReconciliationPKeyAttrs;
            var ukprn = reconciliationAttributes.Contractor.ContractorNonPKeyAttrs.UKPRN;

            if (ukprn != 0)
            {
                var version = reconciliationAttributes.ClaimVersionNumber;
                var claimTypeName = reconciliationAttributes.ClaimType.ClaimTypeNonPKeyAttrs.ClaimTypeName;
                var period = reconciliationAttributes.Period.PeriodValue;
                var reconciliationType = claimTypeName.ToReconciliationType();
                var reconciliationData = new ReconciliationData(fCReconciliation);
                var periodStart = period.Substring(0, 2);
                var periodEnd = period.Substring(2);

                var savedReconciliation = new Reconciliations(reconciliationData, ukprn, version, reconciliationType, period, $"{reconciliationType.GetDisplayName()} reconciliation for Description for 20{periodStart} to 20{periodEnd}")
                {
                    Id = 1,
                    IsValid = true,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                };

                _mockReconciliationRepository.Setup(
                    rr => rr.Create(It.Is<Reconciliations>(r => r.Ukprn == ukprn)))
                    .ReturnsAsync(savedReconciliation);
            }
        }

        #endregion
    }
}
