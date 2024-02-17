using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Model.ResponseTypes;
using Serilog;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExternalAuthController(IConfiguration configuration , UserManager<IdentityUser> _userManager , IPasswordHasher<IdentityUser> passwordHasher) : ControllerBase
    {

        private readonly UserManager<IdentityUser> _userManager = _userManager; 


        //  private readonly HttpClient httpClient;
        private readonly string _clientId = configuration["Authentication:Google:ClientId"];
        private readonly string _clientSecret = configuration["Authentication:Google:ClientSecret"];
        private readonly HttpClient _httpClient = new();


        [HttpPost]
        public async Task<IActionResult> Authlogin([FromBody] UserDto userDto)
        {

            var IspresentUser = await _userManager.FindByEmailAsync(userDto.Email);

            if (IspresentUser != null) return BadRequest("User Already Present");

            var createUser = await _userManager.CreateAsync(new IdentityUser()
            {
                Email = userDto.Email,
                UserName = userDto.Email,
                PasswordHash = userDto.PasswordHash,
            } , userDto.PasswordHash);

            if(createUser.Succeeded!) return NoContent();

            return Ok(userDto);
        } 




        [HttpPost]
        public async Task<IActionResult> GoogleAuth([FromBody] GoogleRequestType googleRequestType)
        {


            var googleAuthValidate = await VerifyGoogleTokenId(googleRequestType.Credential);

            var newUser = new IdentityUser()
            {
                Email = googleAuthValidate.Email,
                UserName = googleAuthValidate.Email,
               
            };

            //  var emailFound = await _userManager.FindByEmailAsync(googleAuthValidate.Email);

            // if(googleAuthValidate != null && emailFound == null)
            //{
              await _userManager.CreateAsync(newUser,"GoogleAuth@122");

            // return Ok(token);
            //  }

            var passwordHash = passwordHasher.HashPassword(newUser, "passwoD@@89");

            Log.Information("Hash =>  {@passwordHash}", passwordHash);



            return Ok(googleAuthValidate);
        }

































            [HttpPost]
            public Task<IActionResult> FaceBookAuth([FromBody] FacebookRequestType facebookRequestType)
            {
                facebookRequestType.AppId = configuration["Authentication:Facebook:AppId"];
        
                Log.Information("FacebookAuth => {@facebookRequestType}", facebookRequestType);
        
                return Task.FromResult<IActionResult>(Ok(facebookRequestType));
            }
        


        
        private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleTokenId(string token)
        {
            try
            {
                var validationSettings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new string[] { configuration["Authentication:Google:ClientId"] } 
                };
                GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(token, validationSettings);

                return payload;
            }catch(Exception ex) {
                Log.Error("Error => {@ex}" , ex);
            }

            return null;
        }









    }
}
