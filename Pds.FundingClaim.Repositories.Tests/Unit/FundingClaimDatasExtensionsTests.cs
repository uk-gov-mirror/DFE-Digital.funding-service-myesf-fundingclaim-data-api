using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pds.FundingClaim.Repositories.DataModels;
using Pds.FundingClaim.Repositories.Enums;
using Pds.FundingClaim.Repositories.Enums.Support;
using System;

namespace Pds.FundingClaim.Repositories.Tests.Unit
{
    [TestClass]
    public class FundingClaimDatasExtensionsTests
    {
        [TestMethod, TestCategory("Unit")]
        public void SetFundingClaim_WhenFundingClaimDataExists_SetsFundingClaimOnFundingClaimDataObject()
        {
            //arrange
            var window = new FundingClaimWindow();

            var schemaFundingClaim = new CorporateSchema.FundingClaims.FundingClaim()
            {
                FundingClaimId = "1617-YearEnd_12345678_1",
                Ukprn = "12345678",
                VersionNumber = 1,
                ClaimTypeName = "YEAREND",
                Period = "1617",
                SubmissionDateTime = DateTime.Parse("2016-09-01T00:00:00")
            };

            var fundingClaimData = new FundingClaimData(schemaFundingClaim);

            var expected = new DataModels.FundingClaim(
                window,
                fundingClaimData,
                schemaFundingClaim.FundingClaimId,
                int.Parse(schemaFundingClaim.Ukprn),
                schemaFundingClaim.VersionNumber,
                schemaFundingClaim.ClaimTypeName.ToFundingClaimType(),
                schemaFundingClaim.Period,
                schemaFundingClaim.SubmissionDateTime,
                FundingClaimState.ReadyToReview);

            //act
            var actual = fundingClaimData.SetFundingClaim(expected).FundingClaim;

            //arrange
            actual.Should().Be(expected);
        }
    }
}