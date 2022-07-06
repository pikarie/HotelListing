﻿using HotelListing.API.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace HotelListing.API.Middlewares
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;

		public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		/// <summary>
		/// Add a global try catch over all requests which this middleware encompasses.
		/// </summary>
		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Something went wrong in the method {NameOfMethod}.", nameof(context.Request.Method));
				await HandleExceptionAsync(context, e);
			}
		}

		private Task HandleExceptionAsync(HttpContext context, Exception e)
		{
			context.Response.ContentType = "application/json";
			HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

			var errorDetails = new ErrorDetails
			{
				ErrorType = "Failure",
				ErrorMessage = e.Message,
			};

			switch (e)
			{
				case NotFoundException notFoundException:
					statusCode = HttpStatusCode.NotFound;
					errorDetails.ErrorType = "Not Found";
					break;
				default:
					break;
			}

			string response = JsonConvert.SerializeObject(errorDetails);
			context.Response.StatusCode = (int)statusCode;

			return context.Response.WriteAsJsonAsync(response);
		}
	}

	public class ErrorDetails
	{
		public string ErrorType { get;  set; }
		public string ErrorMessage { get; set; }
	}
}
