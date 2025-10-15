using CleanArchitecture.Application.Abstractions;
using CleanArchitecture.Application.Behaviors;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Infrastructure.Authentication;
using CleanArchitecture.Infrastructure.Services;
using CleanArchitecture.Persistance.Context;
using CleanArchitecture.Persistance.Repositories;
using CleanArchitecture.Persistance.Services;
using CleanArchitecture.WebApi.Middleware;
using CleanArchitecture.WebApi.OptionsSetup;
using FluentValidation;
using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);

//service icin scope islemleri 
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

//ts paketinden repository ve uof pattern icin scope islemleri
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<AppDbContext>());

//exception icin scope islemleri
builder.Services.AddTransient<ExceptionMiddleware>();

// appsetings json'deki Jwt bolumunu JwtOptions ile eslestirme
builder.Services.ConfigureOptions<JwtOptionsSetup>();

//configure jwt bearer options setup ile addjwtbearer icindeki konfigurasyon islemlerini ayri bir sinifa tasiyoruz. 
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

//Automapper islemleri
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(CleanArchitecture.Persistance.AssemblyReference).Assembly);
});

//connection islemleri 
string connectionString = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

//identity yapisini service tanitma ve db ile iliskilendirme
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();


//fluent.smtp mail ayarlari
var smtpSettings = builder.Configuration.GetSection("SmtpSettings");
builder.Services.AddFluentEmail(smtpSettings["FromEmail"])
    .AddSmtpSender(new SmtpClient
    {
        Host = smtpSettings["Host"],
        Port = int.Parse(smtpSettings["Port"]),
        EnableSsl = bool.Parse(smtpSettings["EnableSsl"]),
        Credentials = new NetworkCredential(
                    smtpSettings["Username"],
                    smtpSettings["Password"])
    });



builder.Services.AddControllers()
    //Presentation katmaninda controller yazmak icin bu sekilde program.cs'e tanitiyoruz.
    .AddApplicationPart(typeof(CleanArchitecture.Presentation.AssemblyReference).Assembly);

//Cqrs pattern kullanmak icin MediatR kutuphanesini ekliyoruz ve tum handler'lari tarayip kayit ediyoruz. 
builder.Services.AddMediatR(cfr =>
{
    cfr.RegisterServicesFromAssembly(typeof(CleanArchitecture.Application.AssemblyReference).Assembly);
});

//Fluent validation icin MediatR pipeline'a validation behavior ekliyoruz.
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
//Fluent validation icin tum validator'lari tarayip kayit ediyoruz.
builder.Services.AddValidatorsFromAssembly(typeof(CleanArchitecture.Application.AssemblyReference).Assembly);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//exception icin middleware ekleme
app.UseMiddlewareExtensions();

app.UseHttpsRedirection();


app.MapControllers();

app.Run();
