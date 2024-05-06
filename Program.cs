using DotnetHoneyApi.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

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

    app.Run();
}