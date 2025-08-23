using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using MillionApi.Application.Common.Exceptions;
using MongoDB.Driver;

public sealed class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    public GlobalExceptionHandlingMiddleware(
        ILogger<GlobalExceptionHandlingMiddleware> logger,
        ProblemDetailsFactory problemDetailsFactory)
    {
        _logger = logger;
        _problemDetailsFactory = problemDetailsFactory;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            var pd = _problemDetailsFactory.CreateProblemDetails(
                context,
                statusCode: StatusCodes.Status499ClientClosedRequest,
                title: "Request was canceled.",
                detail: "The operation was canceled by the client or server."
            );
            context.Response.StatusCode = pd.Status ?? StatusCodes.Status400BadRequest;
            await WriteProblemDetails(context, pd);
        }
        catch (ValidationException vex)
        {
            _logger.LogWarning(vex, "Validation error");
            var pd = _problemDetailsFactory.CreateProblemDetails(
                context,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Validation failed.",
                detail: vex.Message
            );
            pd.Extensions["errors"] = vex.Errors;
            await WriteProblemDetails(context, pd);
        }
        catch (NotFoundException nfex)
        {
            _logger.LogInformation(nfex, "Resource not found");
            var pd = _problemDetailsFactory.CreateProblemDetails(
                context,
                statusCode: StatusCodes.Status404NotFound,
                title: "Resource not found.",
                detail: nfex.Message
            );
            await WriteProblemDetails(context, pd);
        }
        catch (ConflictException cex)
        {
            _logger.LogWarning(cex, "Conflict");
            var pd = _problemDetailsFactory.CreateProblemDetails(
                context,
                statusCode: StatusCodes.Status409Conflict,
                title: "Conflict.",
                detail: cex.Message
            );
            await WriteProblemDetails(context, pd);
        }
        catch (MongoWriteException mwx) when (mwx.WriteError?.Category == ServerErrorCategory.DuplicateKey)
        {
            _logger.LogWarning(mwx, "Mongo duplicate key");
            var pd = _problemDetailsFactory.CreateProblemDetails(
                context,
                statusCode: StatusCodes.Status409Conflict,
                title: "Duplicate key.",
                detail: "A resource with the same unique key already exists."
            );
            await WriteProblemDetails(context, pd);
        }
        catch (TimeoutException tex)
        {
            _logger.LogError(tex, "Timeout");
            var pd = _problemDetailsFactory.CreateProblemDetails(
                context,
                statusCode: StatusCodes.Status504GatewayTimeout,
                title: "Timeout.",
                detail: "The operation timed out."
            );
            await WriteProblemDetails(context, pd);
        }
        catch (MongoException mex)
        {
            _logger.LogError(mex, "MongoDB error");
            var pd = _problemDetailsFactory.CreateProblemDetails(
                context,
                statusCode: StatusCodes.Status503ServiceUnavailable,
                title: "Database unavailable.",
                detail: "We could not complete the request due to a database error."
            );
            await WriteProblemDetails(context, pd);
        }
        catch (DomainException dex)
        {
            _logger.LogWarning(dex, "Domain rule violated");
            var pd = _problemDetailsFactory.CreateProblemDetails(
                context,
                statusCode: StatusCodes.Status422UnprocessableEntity,
                title: "Domain rule violated.",
                detail: dex.Message
            );
            await WriteProblemDetails(context, pd);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            var pd = _problemDetailsFactory.CreateProblemDetails(
                context,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Unexpected error.",
                detail: "An unexpected error occurred."
            );
            await WriteProblemDetails(context, pd);
        }
    }

    private static async Task WriteProblemDetails(HttpContext context, ProblemDetails pd)
    {
        pd.Instance ??= context.Request.Path;
        pd.Extensions["traceId"] = context.TraceIdentifier;

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = pd.Status ?? StatusCodes.Status500InternalServerError;

        await context.Response.WriteAsJsonAsync(pd);
    }
}
