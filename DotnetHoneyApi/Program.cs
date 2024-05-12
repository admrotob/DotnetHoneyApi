using NLog;
using NLog.Web;
using DotnetHoneyApi.Authentication;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    builder.Host.UseNLog();

    var app = builder.Build();
    {
        app.MapControllers();
        app.UseMiddleware<TrapperMiddleware>();
        app.UseHsts();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
        }

        app.UseAuthorization();

        // Map health check endpoint to allow administrators to deploy in cloud platforms.
        app.MapGet("/healthz", () => "OK");

        app.Run();
    }
}
catch (Exception exception)
{
    // Log setup errors with NLog
    logger.Error(exception, "Stopped program execution because of ecxeption");
    throw;
}
finally
{
    // Clean-up your damn mess NLog
    NLog.LogManager.Shutdown();
}
