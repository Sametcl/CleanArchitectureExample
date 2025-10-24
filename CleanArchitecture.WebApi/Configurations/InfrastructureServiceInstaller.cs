
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using CleanArchitecture.Application.Abstractions;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Infrastructure.Authentication;
using CleanArchitecture.Infrastructure.Storage;
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



            //S3 AYARLARI 
            //appsettings json'deki AwsS3Settings bolumunu okuyup S3Options ile eslestirme
            services.Configure<S3Options>(configuration.GetSection("AWS"));

            /*
            var awsOptions = new AWSOptions();
            configuration.GetSection("AWS").Bind(awsOptions);

            services.AddDefaultAWSOptions(awsOptions);
            //IAmazonS3 servisini DI konteynerine kaydetme 
            services.AddAWSService<IAmazonS3>();
            */

            services.AddSingleton<IAmazonS3>(sp =>
            {
                
                // Ayarları doğrudan IConfiguration'dan (appsettings.Development.json) oku.
                var accessKey = configuration["AWS:AccessKey"];
                var secretKey = configuration["AWS:SecretKey"];
                var region = configuration["AWS:Region"];

                var credentials = new BasicAWSCredentials(accessKey, secretKey);
                var regionEndpoint = RegionEndpoint.GetBySystemName(region);

                // IAmazonS3 istendiğinde, bu nesneyi oluştur ve ver.
                return new AmazonS3Client(credentials, regionEndpoint);
            });

            //Kendi servisimiz olan AwsS3Service'i IAwsS3Service arayuzune baglama
            services.AddScoped<IAwsS3Service, AwsS3Service>();
        }
    }
}
