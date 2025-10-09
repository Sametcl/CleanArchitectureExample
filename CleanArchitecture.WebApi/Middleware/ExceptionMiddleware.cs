


using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Persistance.Context;
using FluentValidation;

namespace CleanArchitecture.WebApi.Middleware
{
    public sealed class ExceptionMiddleware : IMiddleware
    {
        private readonly AppDbContext _context;

        public ExceptionMiddleware(AppDbContext context)
        {
            _context = context;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await LogExceptionToDataBaseAsync(ex, context.Request);
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;
            if (ex is ValidationException validationException)
            {
                return context.Response.WriteAsync(new ValidationErrorDetails
                {
                    Errors = ((ValidationException)ex).Errors.Select(s=>s.ErrorMessage),
                    StatusCode = 400 
                }.ToString());
            }
            return context.Response.WriteAsync(new ErrorResult
            {
                Message = ex.Message,
                StatusCode = context.Response.StatusCode
            }.ToString());
        }

        private async Task LogExceptionToDataBaseAsync(Exception ex , HttpRequest request)
        {
            ErrorLog errorLog = new ErrorLog
            {
                ErrorMessage = ex.Message,
                StackTrace = ex.StackTrace,
                RequestPath = request.Path,
                RequestMethod = request.Method,
                Timestamp = DateTime.Now
            };
            await _context.Set<ErrorLog>().AddAsync(errorLog,default);
            await _context.SaveChangesAsync(default);
        }
    }
}
