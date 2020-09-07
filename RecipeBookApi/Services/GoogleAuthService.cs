using Common.Factories;
using Common.Models;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RecipeBookApi.Models;
using RecipeBookApi.Options;
using RecipeBookApi.Services.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBookApi.Services
{
	public class GoogleAuthService : IAuthService
	{
		private readonly AppOptions _appOptions;
		private IAppUsersService _appUserStorage;

		public GoogleAuthService(IOptionsSnapshot<AppOptions> appOptions, IAppUsersService appUserStorage)
		{
			_appOptions = appOptions.Value;
			_appUserStorage = appUserStorage;
		}

		public async Task<string> Authenticate(string token)
		{
			var googleAuthPayload = await GoogleJsonWebSignature.ValidateAsync(token, new GoogleJsonWebSignature.ValidationSettings());
			var user = UpdateDbWithUserPayload(googleAuthPayload);
			var userToken = CreateUserToken(user);

			return userToken;
		}

		public AppUserClaimModel GetUserFromClaims(ClaimsPrincipal userClaims)
		{
			return new AppUserClaimModel
			{
				AppUserId = int.Parse(CryptoFactory.Decrypt(_appOptions.GoogleClientSecret, userClaims.FindFirst(nameof(AppUserClaimModel.AppUserId)).Value)),
				Username = userClaims.FindFirst(nameof(AppUserClaimModel.Username)).Value,
				FirstName = userClaims.FindFirst(nameof(AppUserClaimModel.FirstName)).Value,
				LastName = userClaims.FindFirst(nameof(AppUserClaimModel.LastName)).Value
				//CanViewRecipe = Convert.ToBoolean(userClaims.FindFirst(nameof(AppUserClaimModel.CanViewRecipe)).Value),
				//CanEditRecipe = Convert.ToBoolean(userClaims.FindFirst(nameof(AppUserClaimModel.CanEditRecipe)).Value),
				//IsAdmin = Convert.ToBoolean(userClaims.FindFirst(nameof(AppUserClaimModel.IsAdmin)).Value)
			};
		}

		private AppUser UpdateDbWithUserPayload(GoogleJsonWebSignature.Payload googleAuthPayload)
		{
			var user = _appUserStorage.Get(googleAuthPayload.Email);
			if (user == null)
			{
				user = new AppUser
				{
					Username = googleAuthPayload.Email,
					FirstName = googleAuthPayload.GivenName,
					LastName = googleAuthPayload.FamilyName
				};

				user.AppUserId = _appUserStorage.Create(user);
			}
			else
			{
				user.LastLoggedInDate = DateTime.UtcNow;

				_appUserStorage.UpdateLastLoggedInDate(user);
			}

			return user;
		}

		private string CreateUserToken(AppUser appUser)
		{
			var claims = new List<Claim>
			{
				new Claim(nameof(AppUserClaimModel.AppUserId), CryptoFactory.Encrypt(_appOptions.GoogleClientSecret, appUser.AppUserId.ToString())),
				new Claim(nameof(AppUserClaimModel.Username), appUser.Username),
				new Claim(nameof(AppUserClaimModel.FirstName), appUser.FirstName),
				new Claim(nameof(AppUserClaimModel.LastName), appUser.LastName)
				//new Claim(nameof(AppUserClaimModel.CanViewRecipe), appUser.CanViewRecipe.ToString()),
				//new Claim(nameof(AppUserClaimModel.CanEditRecipe), appUser.CanEditRecipe.ToString()),
				//new Claim(nameof(AppUserClaimModel.IsAdmin), appUser.IsAdmin.ToString())
			};

			var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appOptions.GoogleClientSecret));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var tokenExpireDateTime = DateTime.UtcNow.AddMonths(6);

			var token = new JwtSecurityToken(null, null, claims, null, tokenExpireDateTime, credentials);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
