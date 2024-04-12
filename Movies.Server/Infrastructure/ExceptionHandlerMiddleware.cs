using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Movies.Server.Infrastructure
{
	/// <summary>
	/// Active on actual servers, if debug log tracing is needed.
	/// </summary>
	public class ExceptionHandlerMiddleware
	{
		private readonly RequestDelegate _next;
		
		public ExceptionHandlerMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
		{
			try
			{
				await _next(context);
			}
			catch (Exception e)
			{
				int statusCode = 500;
				string message =
					"The server encountered an unexpected error and cannot fulfill the request at this time.";
				if (e is UnauthorizedAccessException)
				{
					statusCode = 401;
					message = "Request not completed as valid credentials are not provided.";
				}
				/*else
				{
					EmailSender.Send(context, configuration, e);
				}*/

				context.Response.StatusCode = statusCode;
				context.Response.ContentType = "text/plain";
				await context.Response.WriteAsync(message);
			}
		}
	}
}