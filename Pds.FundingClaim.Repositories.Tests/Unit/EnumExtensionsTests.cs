using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pds.FundingClaim.Repositories.Enums;
using Pds.FundingClaim.Repositories.Enums.Support;
using System;

namespace Pds.FundingClaim.Repositories.Tests.Unit
{
    [TestClass]
    public class EnumExtensionsTests
    {
        private enum TestEnum
        {
            A,
            B,
            C
        }

        private struct TestStruct
        {
        }

        #region ToFundingClaimType

        [TestMethod, TestCategory("Unit")]
        public void ToFundingClaimType_WhenCalledOnValidFundingClaimTypeString_ReturnsCorrectEnum()
        {
            //arrange
            var fundingClaimTypeString = "yearend";
            var expected = FundingClaimType.YEAREND;

            //act
            var actual = fundingClaimTypeString.ToFundingClaimType();

            //assert
            actual.Should().Be(expected);
        }

        [TestMethod, TestCategory("Unit")]
        public void ToFundingClaimType_WhenCalledOnValidFundingClaimTypeStringWithSpaces_ReturnsCorrectEnum()
        {
            //arrange
            var fundingClaimTypeString = "Year End";
            var expected = FundingClaimType.YEAREND;

            //act
            var actual = fundingClaimTypeString.ToFundingClaimType();

            //assert
            actual.Should().Be(expected);
        }

        [TestMethod, TestCategory("Unit")]
        public void ToFundingClaimType_WhenCalledOnInvalidFundingClaimType_ThrowsException()
        {
            //arrange
            var fundingClaimTypeString = "year";

            //act & assert
            fundingClaimTypeString.Invoking(f => f.ToFundingClaimType())
                .Should().ThrowExactly<ArgumentException>()
                .WithMessage("Requested value 'YEAR' was not found.");
        }

        #endregion


        #region ToReconciliationType

        [TestMethod, TestCategory("Unit")]
        public void ToReconciliationType_WhenCalledOnValidReconciliationTypeString_ReturnsCorrectEnum()
        {
            //arrange
            var reconciliationTypeString = "yearend";
            var expected = ReconciliationType.YEAREND;

            //act
            var actual = reconciliationTypeString.ToReconciliationType();

            //assert
            actual.Should().Be(expected);
        }

        [TestMethod, TestCategory("Unit")]
        public void ToReconciliationType_WhenCalledOnValidReconciliationTypeStringWithSpaces_ReturnsCorrectEnum()
        {
            //arrange
            var reconciliationTypeString = "Mid Year";
            var expected = ReconciliationType.MIDYEAR;

            //act
            var actual = reconciliationTypeString.ToReconciliationType();

            //assert
            actual.Should().Be(expected);
        }

        [TestMethod, TestCategory("Unit")]
        public void ToReconciliationType_WhenCalledOnInvalidReconciliationType_ThrowsException()
        {
            //arrange
            var reconciliationTypeString = "year";

            //act & assert
            reconciliationTypeString.Invoking(r => r.ToReconciliationType())
                .Should().ThrowExactly<ArgumentException>()
                .WithMessage("Requested value 'YEAR' was not found.");
        }

        #endregion

        #region GetDisplayName

        [TestMethod, TestCategory("Unit")]
        public void GetDisplayName_WhenCalledOnStructWithNoMember_ThrowsException()
        {
            //arrange
            var testStruct = default(TestStruct);

            //act & assert
            testStruct.Invoking(t => t.GetDisplayName())
                .Should().ThrowExactly<ArgumentException>()
                .WithMessage("error 'Pds.FundingClaim.Repositories.Tests.Unit.EnumExtensionsTests+TestStruct' not found in type 'TestStruct'");
        }

        [TestMethod, TestCategory("Unit")]
        public void GetDisplayName_WhenCalledOnEnumWithNoDisplayAttribute_ThrowsException()
        {
            //act & assert
            TestEnum.A.Invoking(t => t.GetDisplayName())
                .Should().ThrowExactly<ArgumentException>()
                .WithMessage("'TestEnum.A' doesn't have DisplayAttribute");
        }

        #endregion
    }
}