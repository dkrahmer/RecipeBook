using Microsoft.AspNetCore.Mvc;
using RecipeBookApi.Logic.Contracts;
using RecipeBookApi.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace RecipeBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly IAppUserLogic _appUserLogic;

        public AppUserController(IAppUserLogic appUserLogic)
        {
            _appUserLogic = appUserLogic;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<AppUserViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllAppUsers()
        {
            var allAppUsers = await _appUserLogic.GetAll();

            return Ok(allAppUsers);
        }
    }
}