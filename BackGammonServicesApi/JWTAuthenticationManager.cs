using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace BackGammonServicesApi
{
    public class JWTAuthenticationManager
    {
        private readonly string key;
        private readonly IDictionary<string, string> users = new Dictionary<string, string>()
        { {"test","password" },{"test1","pwd"} };
        public JWTAuthenticationManager(string _key)
        {
            this.key = _key;
        }
        public string Authenticate(string username, string password)
        {
            if (!users.Any(u=>u.Key==username&& u.Value== password))
            { return null; }

            JwtSecurityTokenHandler tokenhandler = new JwtSecurityTokenHandler();
            var tokenKey=Encoding.ASCII.GetBytes(key);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            }; 
            var token=tokenhandler.CreateToken(tokenDescriptor);
            return tokenhandler.WriteToken(token);
        }
    }
}
