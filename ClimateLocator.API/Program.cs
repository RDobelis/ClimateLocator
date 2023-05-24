using ClimateLocator.Core.Interfaces;
using ClimateLocator.Data;
using ClimateLocator.Services;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ClimateLocatorDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ClimateLocatorDb"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ClimateLocatorDb"))));

builder.Services.AddHttpClient("Ip2LocationService", client =>
{
    client.BaseAddress = new Uri("https://api.ip2location.io/");
});

builder.Services.AddHttpClient("WeatherbitService", client =>
{
    client.BaseAddress = new Uri("https://api.weatherbit.io/v2.0/");
});

builder.Services.AddScoped<Ip2LocationService>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var configuration = sp.GetRequiredService<IConfiguration>();
    var httpClient = httpClientFactory.CreateClient("Ip2LocationService");
    var apiKey = configuration["GeolocationApiKey"];
    return new Ip2LocationService(httpClient, apiKey);
});

builder.Services.AddScoped<WeatherbitService>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var configuration = sp.GetRequiredService<IConfiguration>();
    var httpClient = httpClientFactory.CreateClient("WeatherbitService");
    var apiKey = configuration["WeatherApiKey"];
    return new WeatherbitService(httpClient, apiKey);
});

builder.Services.AddMemoryCache();

builder.Services.AddScoped<IDataStorageService, DataStorageService>();
builder.Services.AddScoped<WeatherService>();

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