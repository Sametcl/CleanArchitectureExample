
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Net.Mail;

namespace CleanArchitecture.WebApi.Configurations
{
    public sealed class PresentationServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration, IHostBuilder host)
        {

            //Cors politikasi
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            ////fluent.smtp mail ayarlari
            //var smtpSettings = configuration.GetSection("SmtpSettings");
            //services.AddFluentEmail(smtpSettings["FromEmail"])
            //    .AddSmtpSender(new SmtpClient
            //    {
            //        Host = smtpSettings["Host"],
            //        Port = int.Parse(smtpSettings["Port"]),
            //        EnableSsl = bool.Parse(smtpSettings["EnableSsl"]),
            //        Credentials = new NetworkCredential(
            //                    smtpSettings["Username"],
            //                    smtpSettings["Password"])
            //    });


            services.AddControllers()
                //Presentation katmaninda controller yazmak icin bu sekilde program.cs'e tanitiyoruz.
                .AddApplicationPart(typeof(CleanArchitecture.Presentation.AssemblyReference).Assembly);


            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(setup =>
            {
                var jwtSecuritySheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Put **_ONLY_** yourt JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                setup.AddSecurityDefinition(jwtSecuritySheme.Reference.Id, jwtSecuritySheme);

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecuritySheme, Array.Empty<string>() }
                });
            });
        }
    }
}
