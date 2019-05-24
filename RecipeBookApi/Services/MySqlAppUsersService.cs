using Common.Models;
using Common.MySql;
using Microsoft.Extensions.Options;
using RecipeBookApi.Options;
using RecipeBookApi.Services.Contracts;
using System;
using System.Linq;

namespace RecipeBookApi.Services
{
	public class MySqlAppUsersService : IAppUsersService
	{
		private AppOptions _options;
		public MySqlAppUsersService(IOptions<AppOptions> options)
		{
			_options = options.Value;
		}

		public int Create(AppUser appUser)
		{
			appUser.CreateDateTime = appUser.UpdateDateTime = DateTime.UtcNow;
			appUser.LastLoggedInDate = appUser.CreateDateTime;

			using (var db = new MySqlDbContext(_options.MySqlConnectionString))
			{
				var addedAppUser = db.Add(appUser);
				db.SaveChanges();
				return addedAppUser.Entity.AppUserId;
			}
		}

		public AppUser Get(string username)
		{
			using (var db = new MySqlDbContext(_options.MySqlConnectionString))
			{
				var appUser = db.AppUsers.Where(u => u.Username == username).FirstOrDefault();

				if (appUser != null)
				{
					appUser.CreateDateTime = DateTime.SpecifyKind(appUser.CreateDateTime, DateTimeKind.Utc);
					appUser.UpdateDateTime = DateTime.SpecifyKind(appUser.UpdateDateTime, DateTimeKind.Utc);
					if (appUser.LastLoggedInDate != null)
						appUser.LastLoggedInDate = DateTime.SpecifyKind(appUser.LastLoggedInDate.Value, DateTimeKind.Utc);
				}

				return appUser;
			}
		}

		public void UpdateLastLoggedInDate(AppUser appUser)
		{
			appUser.UpdateDateTime = DateTime.UtcNow;
			using (var db = new MySqlDbContext(_options.MySqlConnectionString))
			{
				db.AppUsers.Attach(appUser);
				db.Entry(appUser).Property(u => u.LastLoggedInDate).IsModified = true;
				//var updatededRecipe = db.Update(appUser);
				db.SaveChanges();
			}
		}
	}
}
