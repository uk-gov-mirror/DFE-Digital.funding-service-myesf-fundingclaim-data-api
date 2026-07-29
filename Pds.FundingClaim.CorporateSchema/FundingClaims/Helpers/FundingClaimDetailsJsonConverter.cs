using Newtonsoft.Json;

namespace Pds.FundingClaim.CorporateSchema.FundingClaims
{
    /// <summary>
    /// Helpers to the <see cref="FundingClaimDetails"/> class.
    /// </summary>
    public static class FundingClaimDetailsJsonConverter
    {
        /// <summary>
        /// Converts the json data retrieved from the FundingClaimWindows table to a schema Funding Claim Window.
        /// </summary>
        /// <param name="json">String of json data retrieved from the FundingClaimDatas table.</param>
        /// <returns>Schema Funding Claim object.</returns>
        public static FundingClaimDetails FromJson(string json)
        {
            return JsonConvert.DeserializeObject<FundingClaimDetails>(json);
        }

        /// <summary>
        /// Converts the schema Funding Claim to json.
        /// </summary>
        /// <param name="fundingClaimDetails"> Schema funding claim window to be converted to a string.</param>
        /// <returns>json string to store in the FundingClaimWindows table.</returns>
        public static string ToJson(this FundingClaimDetails fundingClaimDetails)
        {
            return JsonConvert.SerializeObject(fundingClaimDetails);
        }
    }
}