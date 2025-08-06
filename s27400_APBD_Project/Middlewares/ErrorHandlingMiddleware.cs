namespace s27400_APBD_Project.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
            _logger.LogError(ex, "An unhandled exception occured");
            await HandleExceptionAsync(context, ex);
        }
    }

    public Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = Convert.ToInt32(exception.Message.Substring(0,3));
        context.Response.ContentType = "application/json";

        var resp = new
        {
            error = new
            {
                message = "Wystąpił nieoczekiwany błąd podczas przetwarzania żądania!",
                detail = exception.Message.Substring(4)
            }
        };

        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(resp);
        return context.Response.WriteAsync(jsonResponse);
    }
}