using Newtonsoft.Json;

namespace Pds.FundingClaim.CorporateSchema.FundingClaims
{
    /// <summary>
    /// Helpers to the <see cref="FundingClaim"/> class.
    /// </summary>
    public static class FundingClaimJsonConverter
    {
        /// <summary>
        /// Converts the json data retrieved from the FundingClaimDatas table to a schema Funding Claim.
        /// </summary>
        /// <param name="json">String of json data retrieved from the FundingClaimDatas table.</param>
        /// <returns>Schema Funding Claim object.</returns>
        public static FundingClaim FromJson(string json)
        {
            return JsonConvert.DeserializeObject<FundingClaim>(json);
        }

        /// <summary>
        /// Converts the schema Funding Claim to json.
        /// </summary>
        /// <param name="fundingClaim">Corporate Schema funding claim to be converted to a string.</param>
        /// <returns>json string to store in the FundingClaimDatas table.</returns>
        public static string ToJson(this FundingClaim fundingClaim)
        {
            return JsonConvert.SerializeObject(fundingClaim);
        }
    }
}