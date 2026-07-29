using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pds.FundingClaim.Repositories.Data;
using Pds.FundingClaim.Repositories.DataModels;
using Pds.FundingClaim.Repositories.Implementation;
using System;
using System.Collections.Generic;

namespace Pds.FundingClaim.Repositories.Tests.Integration
{
    [TestClass]
    public class FundingClaimWindowRepositoryTests
    {
        private readonly PdsContext _context;

        public FundingClaimWindowRepositoryTests()
        {
            _context = new InMemoryDbContextFactory().GetPdsDbContext();
        }

        [TestMethod, TestCategory("Integration")]
        public void GetLastWindow_WhenCalled_GetsLasTWindowWhoseSignatureCloseDateIsPassed()
        {
            //arrange
            var now = new DateTime(2000, 12, 12, 10, 10, 10);

            var oldestFundingClaimWindow = new FundingClaimWindow { Id = 1, SignatureCloseDate = now.AddDays(-2), DataCollectionKey = "2000-Final" };
            var lastFundingClaimWindow = new FundingClaimWindow { Id = 2, SignatureCloseDate = now.AddDays(-1), DataCollectionKey = "2000-Final" };
            var currentFundingClaimWindow = new FundingClaimWindow { Id = 3, SignatureCloseDate = now.AddDays(1), DataCollectionKey = "2000-Final" };

            _context.FundingClaimWindows.AddRange(new List<FundingClaimWindow> { oldestFundingClaimWindow, lastFundingClaimWindow, currentFundingClaimWindow });
            _context.SaveChanges();

            var repository = new FundingClaimWindowRepository(_context, null);

            //act
            var response = repository.GetLastWindow(now);

            //assert
            response.Should().BeEquivalentTo(lastFundingClaimWindow);
        }
    }
}