using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Core.IO;
using Microsoft.AspNetCore.Http;

namespace WebApi.Logging
{
    public class LoggingMiddleware
    {
        private static readonly PathString IncludePathString = new PathString("/api");

        private readonly RequestDelegate _next;
        private readonly StreamPool _streamPool;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
            _streamPool = new StreamPool(new PoolSizeSettings());
        }

        public async Task Invoke(HttpContext context)
        {
            // log only api requests
            if (!context.Request.Path.StartsWithSegments(IncludePathString))
            {
                await _next.Invoke(context).ConfigureAwait(false);
                return;
            }

            var request = context.Request;
            var response = context.Response;

            var sniffedRequestBody = _streamPool.GetStream();
            var sniffedResponseBody = _streamPool.GetStream();
            request.Body = Sniff(request.Body, sniffedRequestBody);
            response.Body = Sniff(response.Body, sniffedResponseBody);

            var buffer = new StringBuilder(2048);
            AppendRequestHeaders(request, buffer); // log request's info before Next.Invoke() called

            try
            {
                await _next.Invoke(context).ConfigureAwait(false);
            }
            finally
            {
                AppendBody(sniffedRequestBody, buffer);

                AppendResponseHeaders(response, buffer);
                AppendBody(sniffedResponseBody, buffer);

                Log(buffer);

                sniffedRequestBody.Dispose();
                sniffedResponseBody.Dispose();
            }
        }

        private static Stream Sniff(Stream sniffWhat, Stream sniffWhere)
        {
            return new SnifferStream(sniffWhat, sniffWhere);
        }

        private static void AppendRequestHeaders(HttpRequest request, StringBuilder buffer)
        {
            var method = request.Method;
            var uri = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
            var protocol = request.Protocol;

            var connection = request.HttpContext.Connection;
            var remoteAddress = $"{connection.RemoteIpAddress}:{connection.RemotePort}";

            buffer.AppendLine($"{method} {uri} {protocol} from {remoteAddress}");

            AppendHeaders(request.Headers, buffer);
        }

        private static void AppendResponseHeaders(HttpResponse response, StringBuilder buffer)
        {
            buffer.AppendLine($"{response.StatusCode}");

            AppendHeaders(response.Headers, buffer);
        }

        private static void AppendHeaders(IHeaderDictionary headers, StringBuilder buffer)
        {
            foreach (var header in headers)
            {
                var key = header.Key;
                var value = string.Join(", ", header.Value);
                buffer.AppendLine($"{key}: {value}");
            }
        }

        private static void AppendBody(MemoryStream body, StringBuilder buffer)
        {
            body.Position = 0;

            try
            {
                var bodyReader = new StreamReader(body);
                var bodyText = bodyReader.ReadToEnd();

                buffer.AppendLine();
                if (!string.IsNullOrEmpty(bodyText))
                {
                    buffer.AppendLine(bodyText);
                }
                buffer.AppendLine();
            }
            catch (Exception ex)
            {
                AppendBodyReadingException(ex, buffer);
            }
            finally
            {
                body.Position = 0;
            }
        }

        private static void AppendBodyReadingException(Exception ex, StringBuilder sb)
        {
            const string clarification = "--- this is not the part of the request or response ---";
            var exceptionType = ex
                .GetType()
                .FullName;

            sb.AppendLine();
            sb.AppendLine(clarification);
            sb.AppendFormat("Failed to read the body: {0}\r\n", exceptionType);
            sb.AppendLine(ex.Message);
            sb.AppendLine(ex.StackTrace);
            sb.Append(clarification);
        }

        private static void Log(StringBuilder builder)
        {
            if (builder != null)
            {
                var logger = Loggers.Requests;
                logger.Debug(builder.ToString());
            }
        }
    }

}