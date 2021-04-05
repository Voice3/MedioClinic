﻿using CMS.DataEngine;
using CMS.SiteProvider;
using MedioClinic.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MedioClinic.Extensions
{
	public static class ApplicationBuilderExtensions
	{
		/// <summary>
		/// Attaches the culture recognition middleware to the request pipeline.
		/// </summary>
		/// <param name="builder">Application builder.</param>
		/// <returns>Modified application builder.</returns>
		public static IApplicationBuilder UseRequestCulture(this IApplicationBuilder builder)
		{
			if (builder is null)
			{
				throw new ArgumentNullException(nameof(builder));
			}

			return builder.UseMiddleware<CultureMiddleware>();
		}

		public static IApplicationBuilder UseLocalizedStatusCodePagesWithReExecute(
	this IApplicationBuilder app,
	string localizedPathFormat,
	string? queryFormat = default)
		{
			if (app == null)
			{
				throw new ArgumentNullException(nameof(app));
			}

			return app.UseStatusCodePages(async context =>
			{
				var originalPath = context.HttpContext.Request.Path;
				var originalQueryString = context.HttpContext.Request.QueryString;

				// Store the original paths so the app can check it.
				context.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(new StatusCodeReExecuteFeature()
				{
					OriginalPathBase = context.HttpContext.Request.PathBase.Value,
					OriginalPath = originalPath.Value,
					OriginalQueryString = originalQueryString.HasValue ? originalQueryString.Value : null,
				});

				var firstSegment = Regex.Match(originalPath, @"/([^/]+)/");
				var segmentValue = firstSegment.Groups[1]?.Value;
				string? culture = default;

				if (firstSegment.Success && !string.IsNullOrWhiteSpace(segmentValue))
				{
					var currentSiteName = SiteContext.CurrentSiteName;
					var siteInfoIdentifier = new SiteInfoIdentifier(currentSiteName);
					var defaultCulture = SettingsKeyInfoProvider.GetSettingsKeyInfo("CMSDefaultCultureCode", siteInfoIdentifier)?.KeyValue;

					culture = CultureSiteInfoProvider.IsCultureOnSite(segmentValue, currentSiteName)
						? segmentValue
						: defaultCulture?.ToLowerInvariant();
				}

				var cultureRouteValue = culture ?? "en-us";

				var newPath = new PathString(
					string.Format(CultureInfo.InvariantCulture, localizedPathFormat, cultureRouteValue, context.HttpContext.Response.StatusCode));

				var formatedQueryString = queryFormat == null ? null :
					string.Format(CultureInfo.InvariantCulture, queryFormat, context.HttpContext.Response.StatusCode);

				var newQueryString = queryFormat == null ? QueryString.Empty : new QueryString(formatedQueryString);

				await ReExecuteRequest(context, originalPath, originalQueryString, newPath, newQueryString);
			});
		}

		private static async Task ReExecuteRequest(StatusCodeContext context, PathString originalPath, QueryString originalQueryString, PathString newPath, QueryString newQueryString)
		{
			// An endpoint may have already been set. Since we're going to re-invoke the middleware pipeline we need to reset
			// the endpoint and route values to ensure things are re-calculated.
			context.HttpContext.SetEndpoint(endpoint: null);
			var routeValuesFeature = context.HttpContext.Features.Get<IRouteValuesFeature>();
			routeValuesFeature?.RouteValues?.Clear();

			context.HttpContext.Request.Path = newPath;
			context.HttpContext.Request.QueryString = newQueryString;

			try
			{
				await context.Next(context.HttpContext);
			}
			finally
			{
				context.HttpContext.Request.QueryString = originalQueryString;
				context.HttpContext.Request.Path = originalPath;
				context.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(null!);
			}
		}
	}
}