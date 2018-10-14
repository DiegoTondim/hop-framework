using System;
using Hop.Framework.Core.Date;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hop.Api.Server.Core.Filters
{
    public class DateFilter : IResourceFilter
    {
        private const string TIMEZONE_KEY = "timezone";

        private readonly IDateProvider _dateProvider;
        public bool UseByTimeZoneId { get; set; }

        public DateFilter(IDateProvider dateProvider)
        {
            _dateProvider = dateProvider;
            UseByTimeZoneId = true;
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (context.HttpContext.Request.Headers.ContainsKey(TIMEZONE_KEY))
            {
                if (UseByTimeZoneId)
                {
                    _dateProvider.SetTimeZone((int)TimeZoneInfo.FindSystemTimeZoneById(
                            context.HttpContext.Request.Headers[TIMEZONE_KEY])
                        .BaseUtcOffset.TotalMinutes);
                }
                else
                {
                    var time = 0;
                    int.TryParse(context.HttpContext.Request.Headers[TIMEZONE_KEY], out time);
                    _dateProvider.SetTimeZone(time);
                }
            }
        }
    }
}
