using Cs2Tracker.Data;
using Cs2Tracker.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services
builder.Services.AddScoped<CaseService>();
builder.Services.AddHttpClient<SteamService>();

// Background Service - Price updater (runs every 30 minutes)
builder.Services.AddHostedService<PriceUpdateService>();

// CORS - Allow Blazor frontend to call API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5074",   // Blazor HTTP
                "https://localhost:7207")  // Blazor HTTPS
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Enable CORS
app.UseCors("AllowBlazorApp");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
