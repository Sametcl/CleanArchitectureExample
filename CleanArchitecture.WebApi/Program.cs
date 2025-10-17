using CleanArchitecture.WebApi.Configurations;
using CleanArchitecture.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InstallServices(
    builder.Configuration, builder.Host, typeof(IServiceInstaller).Assembly
    );

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Cors politikasi 
app.UseCors();

//exception icin middleware ekleme
app.UseMiddlewareExtensions();

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/deneme", () => "Welcome to Clean Architecture Web API"); 

app.Run();
