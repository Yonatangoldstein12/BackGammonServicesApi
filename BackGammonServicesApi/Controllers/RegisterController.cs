using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BackGammonDAL;
using BackGammonModels;

namespace BackGammonServicesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly UsersRespository _usersRespository;
        public RegisterController()
        {
            _usersRespository = new UsersRespository();
        }
        [HttpPost]
        public IActionResult Register([FromBody] User user)
        {
            _usersRespository.Add(user);
            return Ok(user.UserName + " Created.");
        }
    }
}
