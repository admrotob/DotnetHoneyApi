using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using DotnetHoneyApi.Authentication;


namespace DotnetHoneyApi.Tests
{
    public class HoneyPotTests
    {
        private readonly IHost _host;

        // Create the host object to use for testing
        public HoneyPotTests() 
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var host = new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .UseConfiguration(configuration)
                        .ConfigureServices(services =>
                        {
                            services.AddRouting();
                            services.AddControllers();
                        })
                        .Configure(app =>
                        {
                            app.UseHttpsRedirection();
                            app.UseRouting();
                            app.UseAuthorization();
                            app.UseMiddleware<TrapperMiddleware>();
                            app.UseEndpoints(endpoints =>
                            {
                                endpoints.MapGet("/healthz", () =>
                                    TypedResults.Text("OK"));
                            });
                        });
                })
                .Start();

            _host = host;
        }

        [Fact]
        public async void ValidateHealthChecksAuthorized()
        {
            var server = _host.GetTestServer();

            var context = await server.SendAsync(c =>
            {
                c.Request.Method = HttpMethods.Get;
                c.Request.Path = "/healthz";
            });

            Assert.True(context.Response.StatusCode == StatusCodes.Status200OK);
        }

        [Fact]
        public async void ValidateAnonymousRequestsGetRejected()
        {
            var server = _host.GetTestServer();

            var context = await server.SendAsync(c =>
            {
                c.Request.Method = HttpMethods.Get;
                c.Request.Path = "/v1/sharks";
            });

            Assert.True(context.Response.StatusCode == StatusCodes.Status401Unauthorized);
        }
    }
}