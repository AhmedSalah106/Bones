using Bones_App.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Bones_App.CustomMiddleWare
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleWare(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            try
            {
                await next(context);
            }
            catch (CustomException ex)
            {
                context.Response.StatusCode = ex.ErrorResponse.statusCode;
                context.Response.ContentType = "application/json";

                var ErrorResponseJson = JsonSerializer.Serialize(ex.ErrorResponse);

                await context.Response.WriteAsync(ErrorResponseJson);
            }
            catch(DbUpdateException DbException)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                List<string> errors = new List<string>();
                var InnerException = DbException.InnerException;
                if(InnerException!=null)
                {
                    errors.Add(InnerException.Message);
                }
                else
                {
                    errors.Add("Database Error occurred");
                }
                errors.Add(DbException.Message);

                var ErrorResponse = new ErrorResponse()
                {
                    Errors = errors,
                    statusCode = 500
                };

                var ErrorResponseJson = JsonSerializer.Serialize(ErrorResponse);
                await context.Response.WriteAsync(ErrorResponseJson);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var ErrorResponse = new ErrorResponse()
                {
                    statusCode = 500,
                    Errors = new List<string>
                    {
                        "Internal Server Error",
                        ex.Message
                    }
                };
                var ErrorResponseJson = JsonSerializer.Serialize(ErrorResponse);
                await context.Response.WriteAsync(ErrorResponseJson);
            }
        }

    }
}
