using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Webapi.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private Task HandleException(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            string message = "Internal Server Error";

            List<ValidationFailure> errors;
            if (ex.GetType() == typeof(ValidationException))
            {
                context.Response.StatusCode = 400;
                errors = ((ValidationException)ex).Errors.ToList();
                List<string> errDetail = new List<string>();
                foreach (var error in errors)
                {
                    errDetail.Add(error.PropertyName + ": " + error.ErrorMessage);
                }
                var errResult = JsonConvert.SerializeObject(new { Errors = errDetail }, Formatting.None);
                return context.Response.WriteAsync(errResult);
            }

            if (ex.GetType() == typeof(InvalidOperationException))
            {
                message = ex.Message;
                context.Response.StatusCode = 400;
                var errResult = JsonConvert.SerializeObject(new { Error = message }, Formatting.None);
                return context.Response.WriteAsync(errResult);
            }

            var result = JsonConvert.SerializeObject(new { Error = message }, Formatting.None);
            return context.Response.WriteAsync(result);
        }
    }

    public static class CustomExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}