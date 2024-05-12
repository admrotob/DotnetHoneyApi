using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using DotnetHoneyApi.Models;
using NLog;

namespace DotnetHoneyApi.Authentication
{
    public class TrapperMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TrapperMiddleware> _logger;

        public TrapperMiddleware(RequestDelegate next, ILogger<TrapperMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var host = (context.Request.Host).ToString();
            var method = context.Request.Method;
            var path = context.Request.Path;
            var headers = context.Request.Headers;

            // Translate the body in the request and then restore the object to the HTTPContext object
            var objects = await TranslateBody(context.Request.Body);

            var body = objects.Item1;
            context.Request.Body = objects.Item2;

            var logJson = await CreateLogJson(host, method, path, body, headers);

            // Authorize health check requests because they need to be allowed to pass through
            switch(context.Request.Path)
            {
                case "/healthz":
                    _logger.LogInformation(logJson);
                    await _next(context);
                    break;

                case "/nodemanagement":
                    _logger.LogInformation(logJson);
                    await _next(context);
                    break;

                default:
                    _logger.LogInformation(logJson);

                    context.Response.StatusCode = 401;

                    await context.Response.WriteAsync("The Key or Secret Value provided was incorrect. Please check your headers.");
                    break;
            }

            return;
        }

        private async Task<(string, MemoryStream)> TranslateBody(Stream bodyStream)
        {
            var bodyReader = new StreamReader(bodyStream);
            var bodyAsText = await bodyReader.ReadToEndAsync();

            var clonedBodyObject = new MemoryStream();
            await bodyStream.CopyToAsync(clonedBodyObject);


            return (bodyAsText,clonedBodyObject);
        }

        private async Task<string> CreateLogJson(string host, string method, string path, string body, IHeaderDictionary headers)
        {
            var time = DateTime.UtcNow;

            var headerArray = headers.ToArray();
            //string headersString = JsonConvert.SerializeObject(headerArray);

            var log = new LogObject();
            log.RequestTime = time;
            log.RequestBody = body;
            log.RequestHeaders = headerArray;
            log.RequestMethod = method;
            log.RequestPath = path;

            string logJson = JsonConvert.SerializeObject(log);

            return logJson;
        }
    }
}
