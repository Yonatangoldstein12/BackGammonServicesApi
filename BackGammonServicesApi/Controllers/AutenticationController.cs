using BackGammonModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BackGammonServicesApi.Controllers;

[Route("api/v1/authenticate")]
[ApiController]
public class AutenticationController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationUser> _roleManager;

    public AutenticationController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationUser> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    //[HttpPost]
    //[Route("roles/add")]
    ////public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
    //{
    //    var appRole= new ApplicationRole { Name=request.Role};
    //    var CreateRole=await _roleManager.CreateAsync(appRole);
    //    return Ok(new { message = "role created succesfully" });

    //}

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await RegisterAsync(request);
        return result.Success ? Ok(result) : BadRequest(result.Message);

    }
    private async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var userExists= await _userManager.FindByNameAsync(request.UserName);
            if (userExists != null)
                return new RegisterResponse { Message = "user already exists", Success = false };
            userExists = new ApplicationUser
            {
                UserName = request.UserName,
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            var createUserResult= await _userManager.CreateAsync(userExists,request.Password);
            if(!createUserResult.Succeeded) return new RegisterResponse { Message = $"create user faild{createUserResult?.Errors?.First()?.Description}", Success = false };
            //user is created
            var addUserToRoleResult=await _userManager.AddToRoleAsync(userExists,"USER");
            if(!addUserToRoleResult.Succeeded) return new RegisterResponse { Message = $"create user succeeded but could not add user to role{addUserToRoleResult?.Errors?.First()?.Description}", Success = false };
            //working
            return new RegisterResponse
            {
                Success = true,
                Message = "User registed successfuly"
            };
        }
        catch(Exception ex)
        {
            return new RegisterResponse { Message = ex.Message, Success=true };
        }
    }

    [HttpPost]
    [Route("login")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(LoginResponse))]

    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await loginAsync(request);
        return result.Success ? Ok(result) : BadRequest(result.Message);
    }

    private async Task<LoginResponse> loginAsync(LoginRequest request)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user is null)
                return new LoginResponse { Message = "Invalid password", Success = false };

            var claims = new List<Claim>
        {
            new Claim (JwtRegisteredClaimNames.Sub,user.Id.ToString()),
            new Claim(ClaimTypes.Name,user.UserName),
            new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new Claim (ClaimTypes.NameIdentifier,user.Id.ToString())
        };
            var Roles = await _userManager.GetRolesAsync(user);
            var roleClaims = Roles.Select(x => new Claim(ClaimTypes.Role, x));
            claims.AddRange(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1swek3u4uo2u4a6e"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(60);

            var token = new JwtSecurityToken
                (issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: claims,
                expires: expires,
                signingCredentials: creds
                );

            return new LoginResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Message = "login Successful",
                Success = true,
                UserId = user.Id.ToString(),
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new LoginResponse { Success=false,Message=ex.Message};
        }
    }
}
