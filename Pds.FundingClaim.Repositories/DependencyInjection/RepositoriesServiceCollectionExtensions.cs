using Microsoft.Extensions.DependencyInjection;
using Pds.FundingClaim.Repositories.DataModels;
using Pds.FundingClaim.Repositories.Implementation;
using Pds.FundingClaim.Repositories.Interfaces;

namespace Pds.FundingClaim.Repositories.DependencyInjection
{
    /// <summary>
    /// Extensions class for <see cref="IServiceCollection"/> for registering the funding claim repository's services.
    /// </summary>
    public static class RepositoriesServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services for the current feature to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the feature's services to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddRepositoriesServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Setting>, SettingRepository>();
            services.AddScoped<IFundingClaimWindowRepository, FundingClaimWindowRepository>();
            services.AddScoped<IFundingClaimRepository, FundingClaimRepository>();
            services.AddScoped<IRepository<ReconciliationAllocationGroups>, ReconciliationAllocationGroupsRepository>();
            services.AddScoped<IRepository<Reconciliations>, ReconciliationsRepository>();
            return services;
        }
    }
}