using Art.Entities;
using Art.TokenRegisterLogin;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Art.Controllers
{// controller a [Authorize] versem onda private olacaq. Amma hansisa methoda AllowAnonymous yazsam onda gorunecek
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static ArtistReg artistReg = new ArtistReg();
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Artist>> Register(ArtistRegDTO artistRegDTO)
        {
            CreatePasswordHash(artistRegDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

            artistReg.nAME = artistRegDTO.nAME; 
            artistReg.PasswordHash = passwordHash;
            artistReg.PasswordSalt = passwordSalt;
            return Ok(artistReg);   
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(ArtistRegDTO artistRegDTO)
        {
            if(artistReg.nAME != artistRegDTO.nAME)
            {
                return BadRequest("Artist not found.");
            }
            
            if(!VerifyPasswordHash(artistRegDTO.Password, artistReg.PasswordHash, artistReg.PasswordSalt))
            {
                return BadRequest("Wrong Password");
            }
            string token = CreateToken(artistReg);
            return Ok(token);
        }

        private string CreateToken(ArtistReg artistReg)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, artistReg.nAME),
                new Claim(ClaimTypes.Role, "Admin" )

            };
            var Key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha512Signature);

            var Token = new JwtSecurityToken//identity
                (
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(3),
                    signingCredentials: creds
                );
            var jwt  = new JwtSecurityTokenHandler().WriteToken(Token);
            return jwt;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computerHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computerHash.SequenceEqual(passwordHash);
            }
        }
    }
}
