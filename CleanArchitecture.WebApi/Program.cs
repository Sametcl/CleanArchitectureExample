using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Persistance.Context;
using CleanArchitecture.WebApi.Configurations;
using CleanArchitecture.WebApi.Middleware;
using Microsoft.EntityFrameworkCore;

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

app.MapGet("/health", async (AppDbContext context) => 
{
    var cars = await context.Set<CleanArchitecture.Domain.Entities.Car>() 
                            .ToListAsync(); 
    return Results.Ok(new
    {
        status = "healthy",
        timestamp = DateTime.UtcNow,
        databaseCheck = "OK",
        cars = cars 
    });
});

app.Run();
