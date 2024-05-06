using Microsoft.AspNetCore.HttpLogging;
using DotnetHoneyApi.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.ResponseHeaders.Add("MyResponseHeader");
    logging.MediaTypeOptions.AddText("application/json");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
    logging.CombineLogs = true;
});

var app = builder.Build();
{
    app.MapControllers();

    app.UseMiddleware<ApiKeyAuthMiddleware>();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseHsts();
    }
    else
    {
        app.UseHsts();
    }
    app.UseHttpsRedirection();

    app.UseAuthorization();

    // Map health check endpoint to allow administrators to deploy in cloud platforms.
    app.MapGet("/healthz", () => "OK");

    app.Run();
}