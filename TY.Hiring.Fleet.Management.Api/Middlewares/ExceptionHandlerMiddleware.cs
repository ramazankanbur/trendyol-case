using System.Text.Json;
using System.Text.Json.Serialization;
using TY.Hiring.Fleet.Management.Model.Models;

namespace TY.Hiring.Fleet.Management.Api.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Custom log");

                context.Response.ContentType = "application/json";
                var response = new DataResult();
                response.AddMessage($"{context.Request.RouteValues["controller"]}/{context.Request.RouteValues["action"]}");
                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            } 
        }
    } 
}
