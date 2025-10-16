
using CleanArchitecture.Application.Abstractions;
using CleanArchitecture.Infrastructure.Authentication;
using CleanArchitecture.WebApi.OptionsSetup;

namespace CleanArchitecture.WebApi.Configurations
{
    public sealed class InfrastructureServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration, IHostBuilder host)
        {
            services.AddScoped<IJwtProvider, JwtProvider>();
            // appsetings json'deki Jwt bolumunu JwtOptions ile eslestirme
            services.ConfigureOptions<JwtOptionsSetup>();

            //configure jwt bearer options setup ile addjwtbearer icindeki konfigurasyon islemlerini ayri bir sinifa tasiyoruz. 
            services.ConfigureOptions<JwtBearerOptionsSetup>();
        }
    }
}
