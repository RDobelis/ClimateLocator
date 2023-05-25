using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ClimateLocatorDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ClimateLocatorDb"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ClimateLocatorDb"))));


builder.Services.AddScoped<IDataStorageService, DataStorageService>();

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
