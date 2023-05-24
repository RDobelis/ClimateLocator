using ClimateLocator.Core.Interfaces;
using ClimateLocator.Core.Models;
using ClimateLocator.Core.Services;
using ClimateLocator.Data;
using ClimateLocator.Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("ClimateLocatorDb");
builder.Services.AddDbContext<ClimateLocatorDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddHttpClient<IGeolocationService, GeolocationService>(client =>
{
    client.BaseAddress = new Uri("https://api.ip2location.io/");
    client.DefaultRequestHeaders.Add("ApiKey", builder.Configuration["GeolocationApiKey"]);
});

builder.Services.AddHttpClient<IWeatherService, WeatherService>(client =>
{
    client.BaseAddress = new Uri("https://api.weatherbit.io/");
    client.DefaultRequestHeaders.Add("ApiKey", builder.Configuration["WeatherApiKey"]);
});

builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
builder.Services.AddScoped<GeolocationRepository>();
builder.Services.AddScoped<WeatherRepository>();


builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
