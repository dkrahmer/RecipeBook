using Common.Dynamo.Contracts;
using Common.Dynamo.Models;
using RecipeBookApi.Logic.Contracts;
using RecipeBookApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeBookApi.Logic
{
    public class DynamoAppUserLogic : IAppUserLogic
    {
        private readonly IDynamoStorageRepository<AppUser> _appUserStorage;

        public DynamoAppUserLogic(IDynamoStorageRepository<AppUser> appUserStorage)
        {
            _appUserStorage = appUserStorage;
        }

        public async Task<IEnumerable<AppUserViewModel>> GetAll()
        {
            var appUsers = await _appUserStorage.ReadAll();

            return appUsers.Select(u => CreateAppUserViewModel(u));
        }

        private static AppUserViewModel CreateAppUserViewModel(AppUser appUser)
        {
            return new AppUserViewModel
            {
                Id = appUser.Id,
                EmailAddress = appUser.EmailAddress,
                FirstName = appUser.FirstName,
                LastLoggedInDate = appUser.LastLoggedInDate,
                LastName = appUser.LastName
            };
        }
    }
}