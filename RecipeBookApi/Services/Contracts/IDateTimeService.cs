using System;

namespace RecipeBookApi.Services.Contracts
{
    public interface IDateTimeService
    {
        DateTime GetEasternNow();
        DateTime GetTokenExpireTime(int hoursUntilExpire);
    }
}
