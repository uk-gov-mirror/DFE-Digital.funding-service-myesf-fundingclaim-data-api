using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Logging;
using Pds.FundingClaim.Repositories.Data;
using Pds.FundingClaim.Repositories.DataModels;
using Pds.FundingClaim.Repositories.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pds.FundingClaim.Repositories.Tests.Integration
{
    [TestClass]
    public class RepositoryTests
    {
        private readonly PdsContext _context;

        public RepositoryTests()
        {
            _context = new InMemoryDbContextFactory().GetPdsDbContext();
        }

        #region Where Tests

        [TestMethod, TestCategory("Integration")]
        public void Where_WhenCalled_GetsMatchedElementsForTheFilter()
        {
            //arrange
            var now = new DateTime(2000, 12, 12, 10, 10, 10);
            var fundingClaimWindowInPastOne =
                 new FundingClaimWindow
                 {
                     Id = 1,
                     SignatureCloseDate = now.AddHours(-2),
                     FundingClaimsRetrieved = false,
                     DataCollectionKey = "2000-Final"
                 };
            var fundingClaimWindowInPastTwo =
                 new FundingClaimWindow
                 {
                     Id = 2,
                     SignatureCloseDate = now.AddDays(-2),
                     DataCollectionKey = "2000-Final"
                 };
            var fundingClaimWindowCurrent =
                 new FundingClaimWindow
                 {
                     Id = 3,
                     SignatureCloseDate = now.AddHours(2),
                     DataCollectionKey = "2000-Final"
                 };
            _context.FundingClaimWindows.AddRange(new List<FundingClaimWindow> { fundingClaimWindowInPastOne, fundingClaimWindowInPastTwo,  fundingClaimWindowCurrent });
            _context.SaveChanges();

            var repository = new Repository<FundingClaimWindow>(_context, null);

            //act
            var response = repository.Where(fundingclaim => fundingclaim.SignatureCloseDate < now);

            //assert
            response.Should().HaveCount(2);
            response.Should().BeEquivalentTo(new List<FundingClaimWindow> { fundingClaimWindowInPastOne, fundingClaimWindowInPastTwo });
        }

        [TestMethod, TestCategory("Integration")]
        public void Where_WhenCalledWithNoMatch_ReturnsEmptyList()
        {
            //arrange
            var now = new DateTime(2000, 12, 12, 10, 10, 10);
            var fundingClaimWindowInPastOne =
                 new FundingClaimWindow
                 {
                     Id = 1,
                     SignatureCloseDate = now.AddHours(-2),
                     FundingClaimsRetrieved = false,
                     DataCollectionKey = "2000-Final"
                 };
            var fundingClaimWindowInPastTwo =
                 new FundingClaimWindow
                 {
                     Id = 2,
                     SignatureCloseDate = now.AddHours(-10),
                     DataCollectionKey = "2000-Final"
                 };
            _context.FundingClaimWindows.AddRange(new List<FundingClaimWindow> { fundingClaimWindowInPastOne, fundingClaimWindowInPastTwo });
            _context.SaveChanges();

            var repository = new Repository<FundingClaimWindow>(_context, null);

            //act
            var response = repository.Where(fundingclaim => fundingclaim.SignatureCloseDate > now);

            //assert
            response.Should().HaveCount(0);
            response.Should().BeEmpty();
        }

        #endregion

        #region FirstOrDefault Tests

        [TestMethod, TestCategory("Integration")]
        public async Task FirstOrDefault_WhenCalledWithNoMatchForFilter_ReturnsNull()
        {
            //arrange
            var settings = new List<Setting>
            {
                new Setting { Type = 8, Value = "Shouldn't get this", CreatedAt = new DateTime(2000, 10, 10), UpdatedAt = new DateTime(2000, 10, 10) },
                new Setting { Type = 9, Value = "Shouldn't get this", CreatedAt = new DateTime(2000, 10, 10), UpdatedAt = new DateTime(2000, 10, 10) }
            };
            _context.Settings.AddRange(settings);
            _context.SaveChanges();

            var repository = new Repository<Setting>(_context, null);

            //act
            var response = await repository.FirstOrDefault(setting => setting.Type == 10);

            //assert
            response.Should().BeNull();
        }

        [TestMethod, TestCategory("Integration")]
        public async Task FirstOrDefault_WhenCalled_GetsMatchedElementsForTheFilter()
        {
            //arrange
            var now = new DateTime(2000, 12, 12, 10, 10, 10);

            var fundingClaimWindowInPast =
                new FundingClaimWindow
                {
                    Id = 1,
                    DataSetVersionId = Guid.NewGuid(),
                    DataCollectionKey = "A",
                    RequiresSignature = true,
                    SubmissionOpenDate = now.AddDays(-1),
                    SignatureCloseDate = now.AddHours(-2),
                    FundingClaimsRetrieved = false
                };
            var fundingClaimWindowCurrent =
                 new FundingClaimWindow
                 {
                     Id = 2,
                     DataSetVersionId = Guid.NewGuid(),
                     DataCollectionKey = "B",
                     RequiresSignature = true,
                     SubmissionOpenDate = now.AddDays(-1),
                     SignatureCloseDate = now.AddHours(2),
                     FundingClaimsRetrieved = false
                 };
            var fundingClaimWindowCurrentWithFundingClaimsRetrieved =
                 new FundingClaimWindow
                 {
                     Id = 3,
                     DataSetVersionId = Guid.NewGuid(),
                     DataCollectionKey = "C",
                     RequiresSignature = true,
                     SubmissionOpenDate = now.AddDays(-1),
                     SignatureCloseDate = now.AddHours(2),
                     FundingClaimsRetrieved = true
                 };

            var fundingClaimWindows = new List<FundingClaimWindow>
            {
               fundingClaimWindowInPast,
               fundingClaimWindowCurrent,
               fundingClaimWindowCurrentWithFundingClaimsRetrieved
            };
            _context.FundingClaimWindows.AddRange(fundingClaimWindows);
            _context.SaveChanges();

            var repository = new Repository<FundingClaimWindow>(_context, null);

            //act
            var response = await repository.FirstOrDefault(window =>
                window.FundingClaimsRetrieved == false
                && window.SubmissionOpenDate <= now
                && now <= window.SignatureCloseDate.Value);

            //assert
            response.Should().BeEquivalentTo(fundingClaimWindowCurrent);
        }

        #endregion


        #region GetAll Tests

        [TestMethod, TestCategory("Integration")]
        public async Task GetAll_WhenCalled_GetsAllElementsFromDatabase()
        {
            //arrange
            var now = new DateTime(2000, 12, 12, 10, 10, 10);

            var fundingClaimWindowInPast =
                new FundingClaimWindow
                {
                    Id = 1,
                    DataSetVersionId = Guid.NewGuid(),
                    DataCollectionKey = "A",
                    RequiresSignature = true,
                    SubmissionOpenDate = now.AddDays(-1),
                    SignatureCloseDate = now.AddHours(-2),
                    FundingClaimsRetrieved = false
                };
            var fundingClaimWindowCurrent =
                 new FundingClaimWindow
                 {
                     Id = 2,
                     DataSetVersionId = Guid.NewGuid(),
                     DataCollectionKey = "B",
                     RequiresSignature = true,
                     SubmissionOpenDate = now.AddDays(-1),
                     SignatureCloseDate = now.AddHours(2),
                     FundingClaimsRetrieved = false
                 };
            var fundingClaimWindowCurrentWithFundingClaimsRetrieved =
                 new FundingClaimWindow
                 {
                     Id = 3,
                     DataSetVersionId = Guid.NewGuid(),
                     DataCollectionKey = "C",
                     RequiresSignature = true,
                     SubmissionOpenDate = now.AddDays(-1),
                     SignatureCloseDate = now.AddHours(2),
                     FundingClaimsRetrieved = true
                 };

            var allFundingClaimWindows = new List<FundingClaimWindow>
            {
               fundingClaimWindowInPast,
               fundingClaimWindowCurrent,
               fundingClaimWindowCurrentWithFundingClaimsRetrieved
            };
            _context.FundingClaimWindows.AddRange(allFundingClaimWindows);
            _context.SaveChanges();

            var repository = new Repository<FundingClaimWindow>(_context, null);

            //act
            var response = await repository.GetAll();

            //assert
            response.Should().BeEquivalentTo(allFundingClaimWindows);
        }

        [TestMethod, TestCategory("Integration")]
        public async Task GetAll_WhenCalledWithNoElementsInDatabase_ReturnsEmptyList()
        {
            //arrange
            var repository = new Repository<FundingClaimWindow>(_context, null);

            //act
            var response = await repository.GetAll();

            //assert
            response.Should().BeEmpty();
        }

        #endregion


        #region Create Tests

        [TestMethod, TestCategory("Integration")]
        public async Task Create_WhenCalled_AddsEntityToDatabaseAndCreatesLog()
        {
            //arrange
            var now = new DateTime(2000, 12, 12, 10, 10, 10);

            var fundingClaimWindow =
                new FundingClaimWindow
                {
                    Id = 1,
                    DataSetVersionId = Guid.NewGuid(),
                    DataCollectionKey = "A",
                    RequiresSignature = true,
                    SubmissionOpenDate = now.AddDays(-1),
                    SignatureCloseDate = now.AddHours(-2),
                    FundingClaimsRetrieved = false
                };

            var mockLogger = new Mock<ILoggerAdapter<Repository<FundingClaimWindow>>>();
            var repository = new Repository<FundingClaimWindow>(_context, mockLogger.Object);

            //act
            await repository.Create(fundingClaimWindow);

            //assert
            _context.FundingClaimWindows.Should().HaveCount(1);
            _context.FundingClaimWindows.First().Should().BeEquivalentTo(fundingClaimWindow);

            mockLogger.Verify(l => l.LogInformation(
                "User just created a {entityType} with id {createdEntityId}.",
                fundingClaimWindow.GetType(),
                fundingClaimWindow.GetType().GetProperty("Id").GetValue(fundingClaimWindow)));
        }

        #endregion


        #region Update Tests

        [TestMethod, TestCategory("Integration")]
        public async Task Update_WhenCalled_UpdatesEntityInDatabaseAndCreatesLog()
        {
            //arrange
            var now = new DateTime(2000, 12, 12, 10, 10, 10);

            var fundingClaimWindow =
                new FundingClaimWindow
                {
                    Id = 1,
                    DataSetVersionId = Guid.NewGuid(),
                    DataCollectionKey = "A",
                    RequiresSignature = true,
                    SubmissionOpenDate = now.AddDays(-1),
                    SignatureCloseDate = now.AddHours(-2),
                    FundingClaimsRetrieved = false
                };
            _context.FundingClaimWindows.Add(fundingClaimWindow);
            _context.SaveChanges();

            var mockLogger = new Mock<ILoggerAdapter<Repository<FundingClaimWindow>>>();
            var repository = new Repository<FundingClaimWindow>(_context, mockLogger.Object);
            fundingClaimWindow.DataCollectionKey = "B";
            fundingClaimWindow.FundingClaimsRetrieved = true;

            //act
            await repository.Update(fundingClaimWindow);

            //assert
            _context.FundingClaimWindows.Should().HaveCount(1);
            _context.FundingClaimWindows.First().DataCollectionKey.Should().BeSameAs("B");
            _context.FundingClaimWindows.First().FundingClaimsRetrieved.Should().BeTrue();

            mockLogger.Verify(l => l.LogInformation($"User just updated a {fundingClaimWindow.GetType()} with id {fundingClaimWindow.Id}."));
        }

        #endregion
    }
}