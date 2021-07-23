using Microsoft.AspNetCore.Http;
using mock_json.Models;
using Newtonsoft.Json;
using Serilog;
using System.IO;
using System.Threading.Tasks;

namespace mock_json.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = SetRequest(context);

            //Copy a pointer to the original response body stream
            var originalBodyStream = context.Response.Body;

            using var responseBody = new MemoryStream();

            context.Response.Body = responseBody;

            await _next(context);

            ///var response = await SetResponse(context.Response);

            var loggedRequestResponse = new LoggedData(request, default); /// response

            _logger.Information(JsonConvert.SerializeObject(loggedRequestResponse, Formatting.Indented));

            await responseBody.CopyToAsync(originalBodyStream);
        }

        private LoggedRequest SetRequest(HttpContext context)
            => new()
            {
                IpRequested = context.Connection.RemoteIpAddress.ToString(),
                HttpMethod = context.Request.Method,
                RequestTo = $"{context.Request.Scheme} {context.Request.Host}{context.Request.Path} {context.Request.QueryString}",
                Path = context.Request.Path,
                QueryParams = context.Request.QueryString.Value,
            };

        private async Task<LoggedResponse> SetResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string bodyString = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return new LoggedResponse()
            {
                StatusCode = response.StatusCode,
                Body = bodyString
            };
        }
    }
}