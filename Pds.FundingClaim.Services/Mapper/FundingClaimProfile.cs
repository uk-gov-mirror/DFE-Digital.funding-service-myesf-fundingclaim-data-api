using AutoMapper;

namespace Pds.FundingClaim.Services.Mapper
{
    /// <summary>
    /// Funding Claim Automapper Profile.
    /// </summary>
    public class FundingClaimProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FundingClaimProfile"/> class.
        /// Creates the mappings required for the Funding Claim API.
        /// </summary>
        public FundingClaimProfile()
        {
            CreateMap<Repositories.DataModels.FundingClaimWindow, Models.FundingClaimWindow>();
            CreateMap<Repositories.DataModels.FundingClaim, Models.FundingClaim>();
        }
    }
}