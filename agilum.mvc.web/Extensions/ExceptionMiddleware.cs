using agilum.mvc.web.Interfaces;
using agilum.mvc.web.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Internal;
using Polly.CircuitBreaker;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace agilum.mvc.web.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IAutenticacaoService autenticacaoService)
        {     

            try
            {
                var listaErros = new List<int>{StatusCodes.Status400BadRequest, StatusCodes.Status401Unauthorized, StatusCodes.Status402PaymentRequired,
                    StatusCodes.Status403Forbidden, StatusCodes.Status404NotFound,StatusCodes.Status405MethodNotAllowed,StatusCodes.Status408RequestTimeout,
                StatusCodes.Status406NotAcceptable,StatusCodes.Status409Conflict,StatusCodes.Status500InternalServerError,StatusCodes.Status501NotImplemented,
                StatusCodes.Status503ServiceUnavailable};

                await _next(httpContext);
                //if(listaErros.Any(x => x == httpContext.Response.StatusCode))
                //{
                //    throw new Exception("Erro");
                //}
            }
            catch (CustomHttpRequestException ex)
            {
                HandleRequestExceptionAsync(httpContext, ex.StatusCode);
            }
            catch (ValidationApiException ex)
            {
                HandleRequestExceptionAsync(httpContext, ex.StatusCode);
            }
            catch (ApiException ex)
            {
                HandleRequestExceptionAsync(httpContext, ex.StatusCode);
            }
            catch (BrokenCircuitException ex)
            {
                HandleCircuitBreakerExceptionAsync(httpContext);
            }
            //catch(Exception ex)
            //{
            //   // HandleCircuitBreakerExceptionAsync(httpContext);
            //}

        }

        private static void HandleRequestExceptionAsync(HttpContext context, HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.Unauthorized)
            {

                var path = (new RouteValueDictionary(new { area = "Identity", page = "/Account/Login", ReturnUrl = context.Request.Path.ToString() }));
                
                context.Response.Redirect($"{path}?ReturnUrl={context.Request.Path}");
                return;
            }

            context.Response.StatusCode = (int)statusCode;
        }

        private static void HandleCircuitBreakerExceptionAsync(HttpContext context)
        {
            context.Response.Redirect($"/error/{context.Response.StatusCode}"); 
            //context.Response.Redirect("/sistema-indisponivel");
        }
    }
}
