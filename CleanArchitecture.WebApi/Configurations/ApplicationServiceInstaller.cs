
using CleanArchitecture.Application.Behaviors;
using FluentValidation;
using MediatR;

namespace CleanArchitecture.WebApi.Configurations
{
    public sealed class ApplicationServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration, IHostBuilder host)
        {
            //Cqrs pattern kullanmak icin MediatR kutuphanesini ekliyoruz ve tum handler'lari tarayip kayit ediyoruz. 
            services.AddMediatR(cfr =>
            {
                cfr.RegisterServicesFromAssembly(typeof(CleanArchitecture.Application.AssemblyReference).Assembly);
            });

            //Fluent validation icin MediatR pipeline'a validation behavior ekliyoruz.
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            //Fluent validation icin tum validator'lari tarayip kayit ediyoruz.
            services.AddValidatorsFromAssembly(typeof(CleanArchitecture.Application.AssemblyReference).Assembly);
        }
    }
}
