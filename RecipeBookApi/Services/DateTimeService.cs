using Microsoft.Extensions.Options;
using RecipeBookApi.Options;
using RecipeBookApi.Services.Contracts;
using System;

namespace RecipeBookApi.Services
{
    public class DateTimeService : IDateTimeService
    {
        private readonly AppDateOptions _appDateOptions;

        public DateTimeService(IOptions<AppDateOptions> appDateOptions)
        {
            _appDateOptions = appDateOptions.Value;
        }

        public DateTime GetEasternNow()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(_appDateOptions.EasternTimeZoneId));
        }

        public DateTime GetTokenExpireTime(int hoursUntilExpire)
        {
            var now = DateTime.UtcNow;
            if (!_appDateOptions.UseUtcForTokenExpire)
            {
                now = GetEasternNow();
            }

            return now.AddHours(hoursUntilExpire);
        }
    }
}
