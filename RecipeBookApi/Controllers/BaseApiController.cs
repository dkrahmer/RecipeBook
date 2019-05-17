using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeBookApi.Models;
using RecipeBookApi.Services.Contracts;

namespace RecipeBookApi.Controllers
{
    [Authorize]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected IAuthService AuthService { get; }

        protected AppUserClaimModel CurrentUser => AuthService.GetUserFromClaims(User);

        protected BaseApiController(IAuthService authService)
        {
            AuthService = authService;
        }
    }
}
