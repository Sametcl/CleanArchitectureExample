using CleanArchitecture.Application.Services;
using CleanArchitecture.Persistance.Context;
using CleanArchitecture.Persistance.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

//scope islemleri 
builder.Services.AddScoped<ICarService, CarService>();

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

builder.Services.AddControllers()
    //Presentation katmaninda controller yazmak icin bu sekilde program.cs'e tanitiyoruz.
    .AddApplicationPart(typeof(CleanArchitecture.Presentation.AssemblyReference).Assembly);

//Cqrs pattern kullanmak icin MediatR kutuphanesini ekliyoruz ve tum handler'lari tarayip kayit ediyoruz. 
builder.Services.AddMediatR(cfr =>
{
    cfr.RegisterServicesFromAssembly(typeof(CleanArchitecture.Application.AssemblyReference).Assembly);
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
