using System.Net;
using System.Text.Json;
using FinanceControl.Application.DTOs;
using FinanceControl.Domain.Exceptions;

namespace FinanceControl.API.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        context.Response.ContentType = "application/json";

        var result = exception switch
        {
            DomainException domainEx => (
                StatusCode: (int)HttpStatusCode.BadRequest,
                Result: ResultViewModel.Error(domainEx.Message, 400)
            ),

            ArgumentException argEx => (
                StatusCode: (int)HttpStatusCode.BadRequest,
                Result: ResultViewModel.Error(argEx.Message, 400)
            ),

            UnauthorizedAccessException => (
                StatusCode: (int)HttpStatusCode.Unauthorized,
                Result: ResultViewModel.Unauthorized("Unauthorized access")
            ),

            KeyNotFoundException => (
                StatusCode: (int)HttpStatusCode.NotFound,
                Result: ResultViewModel.NotFound("Resource not found")
            ),

            _ => (
                StatusCode: (int)HttpStatusCode.InternalServerError,
                Result: ResultViewModel.Error("An unexpected error occurred. Please contact support.", 500)
            )
        };

        context.Response.StatusCode = result.StatusCode;

        var json = JsonSerializer.Serialize(result.Result, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}