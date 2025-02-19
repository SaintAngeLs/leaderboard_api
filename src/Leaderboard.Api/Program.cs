using Leaderboard.Api.Options;
using Leaderboard.Core;
using Leaderboard.Application;
using Leaderboard.Infrastructure;
using Leaderboard.Infrastructure.Auth;
using Leaderboard.Infrastructure.Errors;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Leaderboard.Infrastructure.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

builder.Services.Configure<ApiOptions>(builder.Configuration.GetSection("ApiOptions"));

builder.Services.AddCore();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddAuth();
builder.Services.AddErrorHandling();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo 
    {
        Title = "Leaderboard API",
        Version = "v1",
        Description = "API for managing teams and counters in the Leaderboard application."
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        // Serve the spec at /openapi/v1.json (for document "v1")
        c.RouteTemplate = "openapi/{documentName}.json";
    });
    // app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRateLimiting(); 

app.UseErrorHandling();
// app.UseAuth();

app.MapControllers();

app.MapGet("/", (IOptions<ApiOptions> options) => options.Value.Name);

app.MapScalarApiReference(o => o
    .WithTheme(ScalarTheme.DeepSpace));

app.Run();
