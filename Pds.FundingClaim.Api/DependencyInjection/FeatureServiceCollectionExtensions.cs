using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pds.Core.Azure;
using Pds.FundingClaim.Repositories.DependencyInjection;
using Pds.FundingClaim.Services.Implementations;
using Pds.FundingClaim.Services.Interfaces;

namespace Pds.FundingClaim.Api.DependencyInjection
{
    public static class FeatureServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services for the current feature to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the feature's services to.</param>
        /// <param name="configuration">Application configuration.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddFundingClaimServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAzureMessagingServiceBusAllQueues(configuration);
            services.AddScoped<ISettingDataService, SettingDataService>();
            services.AddScoped<IFundingClaimWindowDataService, FundingClaimWindowDataService>();
            services.AddScoped<IFundingClaimDataService, FundingClaimDataService>();
            services.AddScoped<IReconciliationDataService, ReconciliationDataService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISystemProvider, SystemProvider>();

            services.AddRepositoriesServices();

            return services;
        }
    }
}
