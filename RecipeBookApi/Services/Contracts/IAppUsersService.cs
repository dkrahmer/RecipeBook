using Common.Models;
using RecipeBookApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeBookApi.Services.Contracts
{
	public interface IAppUsersService
	{
		int Create(AppUser appUser);
		AppUser Get(string username);
		void UpdateLastLoggedInDate(AppUser appUser);
	}
}
