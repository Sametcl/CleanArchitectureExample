
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace CleanArchitecture.WebApi.Configurations
{
    public sealed class PersistanceServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration, IHostBuilder host)
        {
            //Automapper islemleri
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(CleanArchitecture.Persistance.AssemblyReference).Assembly);
            });

            //connection islemleri 
            string connectionString = configuration.GetConnectionString("SqlConnection");
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            //identity yapisini service tanitma ve db ile iliskilendirme
            services.AddIdentity<User, Role>().AddEntityFrameworkStores<AppDbContext>();

            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Information()
            //    .Enrich.FromLogContext()
            //    .WriteTo.Console()
            //    .WriteTo.File("Logs/logs-.txt", rollingInterval: RollingInterval.Day)
            //     .WriteTo.MSSqlServer(
            //                connectionString: connectionString,
            //                sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true },
            //                columnOptions: null
            //            )
            //    .CreateLogger();

            //host.UseSerilog();
        }
    }
}
