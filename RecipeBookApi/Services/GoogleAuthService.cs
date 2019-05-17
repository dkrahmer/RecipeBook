using Common.Dynamo.Contracts;
using Common.Dynamo.Models;
using Common.Factories;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RecipeBookApi.Models;
using RecipeBookApi.Options;
using RecipeBookApi.Services.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBookApi.Services
{
    public class GoogleAuthService : IAuthService
    {
        private readonly AppGoogleOptions _googleOptions;
        private readonly IDateTimeService _dateTimeService;
        private readonly IDynamoStorageRepository<AppUser> _appUserStorage;

        public GoogleAuthService(IOptions<AppGoogleOptions> appGoogleOptions, IDateTimeService dateTimeService, IDynamoStorageRepository<AppUser> appUserStorage)
        {
            _googleOptions = appGoogleOptions.Value;
            _dateTimeService = dateTimeService;
            _appUserStorage = appUserStorage;
        }

        public async Task<string> Authenticate(string token)
        {
            var googleAuthPayload = await GoogleJsonWebSignature.ValidateAsync(token, new GoogleJsonWebSignature.ValidationSettings());
            var user = await UpdateStorageWithUserPayload(googleAuthPayload);
            var userToken = CreateUserToken(user);

            return userToken;
        }

        public AppUserClaimModel GetUserFromClaims(ClaimsPrincipal userClaims)
        {
            return new AppUserClaimModel
            {
                Id = CryptoFactory.Decrypt(_googleOptions.ClientSecret, userClaims.FindFirst(nameof(AppUserClaimModel.Id)).Value),
                EmailAddress = userClaims.FindFirst(nameof(AppUserClaimModel.EmailAddress)).Value,
                FirstName = userClaims.FindFirst(nameof(AppUserClaimModel.FirstName)).Value,
                LastName = userClaims.FindFirst(nameof(AppUserClaimModel.LastName)).Value,
                IsAdmin = Convert.ToBoolean(userClaims.FindFirst(nameof(AppUserClaimModel.IsAdmin)).Value)
            };
        }

        private async Task<AppUser> UpdateStorageWithUserPayload(GoogleJsonWebSignature.Payload googleAuthPayload)
        {
            var user = (await _appUserStorage.ReadAll(u => u.EmailAddress.ToLower() == googleAuthPayload.Email.ToLower())).SingleOrDefault();            
            if (user == null)
            {
                user = new AppUser
                {
                    EmailAddress = googleAuthPayload.Email,
                    FirstName = googleAuthPayload.GivenName,
                    LastName = googleAuthPayload.FamilyName,
                    LastLoggedInDate = _dateTimeService.GetEasternNow(),
                    CreateDate = _dateTimeService.GetEasternNow(),
                    UpdateDate = _dateTimeService.GetEasternNow()
                };

                user.Id = await _appUserStorage.Create(user);
            }
            else
            {
                user.LastLoggedInDate = _dateTimeService.GetEasternNow();

                await _appUserStorage.Update(user);
            }

            return user;
        }

        private string CreateUserToken(AppUser appUser)
        {           
            var claims = new List<Claim>
            {
                new Claim(nameof(AppUserClaimModel.Id), CryptoFactory.Encrypt(_googleOptions.ClientSecret, appUser.Id)),
                new Claim(nameof(AppUserClaimModel.EmailAddress), appUser.EmailAddress),
                new Claim(nameof(AppUserClaimModel.FirstName), appUser.FirstName),
                new Claim(nameof(AppUserClaimModel.LastName), appUser.LastName),
                new Claim(nameof(AppUserClaimModel.IsAdmin), appUser.IsAdmin.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_googleOptions.ClientSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(null, null, claims, null, _dateTimeService.GetTokenExpireTime(1), credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
