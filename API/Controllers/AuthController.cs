using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Model;
using Model.ResponseTypes;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static Model.ResponseTypes.ExternalResponseType;

namespace API.Controllers;


[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController(UserManager<ApplicationUser> _userManager , RoleManager<IdentityRole> _roleManager , IConfiguration configuration ) : ControllerBase
{


   

    ApplicationUser user = new ApplicationUser();

   // HttpContextAccessor httpContextAccessor;
     //private  List<ApplicationUser> userList =>  _userManager.Users.ToListAsync<ApplicationUser>() ;

   // private readonly Func<Task<List<ApplicationUser>>> userList = async () => await _userManager.Users.ToListAsync<ApplicationUser>();

    















    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterResponse registerResponse)
    {
        
        var userNew = new ApplicationUser()
        {
            Name = registerResponse.Name,
            Email = registerResponse.Email,
            PasswordHash = registerResponse.Password, 
            UserName = registerResponse.Email,
            PhoneNumberConfirmed  = true ,
            PhoneNumber = registerResponse.PhoneNumber,
            CurrentLocationId = registerResponse.CurrentLocationId,
            ProfilePhotoId = registerResponse.ProfilePhotoId,
        };
        var newUser = await _userManager.FindByEmailAsync(userNew.Email);
        if (newUser != null) return StatusCode(409);

        

        var createUser = await _userManager.CreateAsync(userNew!, registerResponse.Password);
        if (!createUser.Succeeded) return BadRequest(new GeneralResponseB(400, "Failed Creating User .. try agian"));

        // Create Admin User 

        var userList = await _userManager.Users.ToListAsync<ApplicationUser>();

        var userPresent =  userList.Count != 1;


        Log.Information("User Present => {@userPresent}", userPresent);
        //var admin = await _roleManager.FindByNameAsync("Admin");
        if (userPresent == false)
        {
            Log.Information("User Present => {@userPresent}", userPresent);
         // if(admin is null ) await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            await _userManager.AddToRoleAsync(userNew!, "Admin");
        }else
        {
            // Create User 
            var user = await _roleManager.FindByNameAsync("User");
            if (user is null) await _roleManager.CreateAsync(new IdentityRole { Name = "User" });

            await _userManager.AddToRoleAsync(userNew!, "User");
        }

        var foundUser = await _userManager.FindByEmailAsync(userNew.Email);
        //return Ok(new GeneralResponseB(200 , "User Created"));

        return await LoginHelper(foundUser);
    }



    [HttpPost] 
    public async Task<IActionResult> Login([FromBody] LoginResponse loginResponse)
    {

        //var userStores = await _userStore.Users.ToListAsync<IdentityUser>();

        var foundUser = await _userManager.FindByEmailAsync(loginResponse.Email);
        var comparePassword = await _userManager.CheckPasswordAsync(foundUser, loginResponse.Password);
        if (!comparePassword || foundUser is null) return BadRequest(new GeneralResponseB(405, "Email/Password incorrect"));

        return await LoginHelper(foundUser);
    }













    [HttpGet]
    public async Task<IActionResult> RefreshUser()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var tokenObj = new JwtSecurityToken(refreshToken);
        var email = tokenObj.Claims.First().Value;
        var foundUser = await _userManager.FindByEmailAsync(email);
        if (email == null || foundUser == null) return BadRequest("Nice try !! You don't even exist");

        var getRefreshToken = await _userManager.GetAuthenticationTokenAsync(foundUser, "Klando", "refresh");

        if (!getRefreshToken.Equals(refreshToken)) return BadRequest("Invalide Request");

        //var foundUser = await _userManager

        return await LoginHelper(foundUser);
    }






    [HttpPost]
    public async Task<IActionResult> GoogleAuth([FromBody] GoogleRequestType googleRequestType)
    {


        var googleAuthValidate = await VerifyGoogleTokenId(googleRequestType.Credential);

        if (googleAuthValidate is null) return Ok(new GeneralResponse(false, "Not Validated By google")); 

        var userNew = new ApplicationUser()
        {
            Name = googleAuthValidate.Name,
            Email = googleAuthValidate.Email,
            PasswordHash = googleAuthValidate.Email ,
            UserName = googleAuthValidate.Email,
            PhoneNumberConfirmed = true,
            PhotoUrl = googleAuthValidate.Picture
            //PhoneNumber = googleAuthValidate.PhoneNumber,
            //CurrentLocationId = googleAuthValidate.CurrentLocationId,
            //ProfilePhotoId = googleAuthValidate.ProfilePhotoId,
        };


        Log.Information("------------------------------------------Pass 1-----------------------------------------------------------");

        var newUser = await _userManager.FindByEmailAsync(userNew.Email);
        if (newUser != null)
        {
            var klando = await _userManager.GetAuthenticationTokenAsync(newUser, "Klando", "refresh");
            var google = await _userManager.GetAuthenticationTokenAsync(newUser, "Google", "refresh");

            // var provider = await _userManager.
          
            Log.Information("------------------------------------------Pass 2-----------------------------------------------------------");
            if (google == null && klando != null) return Ok(new GeneralResponse(false, "Email is Taken"));
            return await LoginHelper(newUser, "Google");
        }

        Log.Information("------------------------------------------Pass 3-----------------------------------------------------------");
        var createUser = await _userManager.CreateAsync(userNew!, (userNew.PasswordHash + Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)).ToString()));
        if (!createUser.Succeeded) return Ok(new GeneralResponseB(400, "Failed Creating User cuz you already exist . "));


        Log.Information("------------------------------------------Pass 4-----------------------------------------------------------");
        // return Ok(googleAuthValidate);

        var userList = await _userManager.Users.ToListAsync<ApplicationUser>();

        Log.Information("------------------------------------------Pass 5-----------------------------------------------------------");


        var userPresent = userList.Count != 1;

        Log.Information("------------------------------------------Pass 6-----------------------------------------------------------");

        //var admin = await _roleManager.FindByNameAsync("Admin");
        if (userPresent == false)
        {
           
            // if(admin is null ) await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            await _userManager.AddToRoleAsync(userNew!, "Admin");
            Log.Information("------------------------------------------Pass 7-----------------------------------------------------------");
        }
        else
        {
            
            // Create User 
            var user = await _roleManager.FindByNameAsync("User");
            if (user is null) await _roleManager.CreateAsync(new IdentityRole { Name = "User" });

            await _userManager.AddToRoleAsync(userNew!, "User");
        }
        Log.Information("------------------------------------------Pass 8-----------------------------------------------------------");

        var foundUser = await _userManager.FindByEmailAsync(userNew.Email);
     
        return await LoginHelper(foundUser, "Google");

    }











































    private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleTokenId(string token)
    {
        try
        {
            var validationSettings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new string[] { configuration["Authentication:Google:ClientId"]}
            };
            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(token, validationSettings);

            return payload;
        }
        catch (Exception ex)
        {
            Log.Error("Error => {@ex}", ex);
        }

        return null;
    }











    private string GenerateAccessToken(ApplicationUser applicationUser)
    {
        var securtitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securtitykey , SecurityAlgorithms.HmacSha256);
        var userClaims = new [] {
            new Claim(ClaimTypes.NameIdentifier , applicationUser.Id),
            new Claim(ClaimTypes.Name , applicationUser.Name) ,
            new Claim(ClaimTypes.Email , applicationUser.Email) ,
            //new Claim(ClaimTypes.PhoneNumber , applicationUser.PhoneNumber) ,
            new Claim(ClaimTypes.Role , GetRole(applicationUser).ToString()),
        
        };

        var token = new JwtSecurityToken(
             issuer: configuration["Jwt:Issuer"],
             audience: configuration["Jwt:Audience"],
             claims: userClaims,
             expires: DateTime.Now.AddDays(1),
             signingCredentials: credentials
             );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }




    private string GenerateRefreshToken(ApplicationUser applicationUser)
    {
        var securtitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securtitykey, SecurityAlgorithms.HmacSha256);
        var userClaims = new[] {
            new Claim(ClaimTypes.Email , applicationUser.Email) ,
        };

        var token = new JwtSecurityToken(
             issuer: configuration["Jwt:Issuer"],
             audience: configuration["Jwt:Audience"],
             claims: userClaims,
             expires: DateTime.Now.AddDays(1),
             signingCredentials: credentials
             );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }







    private async Task<string> GetRole(ApplicationUser foundUser)
    {
        var getUserRole = await _userManager.GetRolesAsync(foundUser);
        var role = getUserRole.First();

        return role;
    }


    private async Task<IActionResult> LoginHelper(ApplicationUser foundUser , string tokenProvider = "Klando" )
    {

        var token = await _userManager.CreateSecurityTokenAsync(foundUser);

 

        // var token2 = await _userManager.GetAuthenticationTokenAsync(foundUser , "Klando_backend", "refresh");
        var refreshToken = GenerateRefreshToken(foundUser);

        // Store refresh token and the login provider . 
        await _userManager.SetAuthenticationTokenAsync(foundUser, tokenProvider, "refresh", refreshToken);
        // Store refresh token in cookies . 
        Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.Now.AddDays(30)
        });


       // var jwttoken = GenerateAccessToken(foundUser);

        //var tokenstring = new JwtSecurityToken(GenerateAccessToken(foundUser));



        return Ok(new LoginResponseithTokens(GenerateAccessToken(foundUser), 1, refreshToken, DateTime.Now));
    }
}




// Generate Refresh Token 

//     private RefreshToken GenerateRefreshToken()
//     {
//         var refreshToken = new RefreshToken
//         {
//             // RandomNumberGenerator from system.Cryptography
//             Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
//             Expires = DateTime.Now.AddDays(1)
//         };
//         return refreshToken;
//     }




//
//        // To Generate a new refresh token .... 
//        [HttpPost("refresh-token")]
//        public async Task<ActionResult<string>> RefreshToken()
//        {
//
//            var refreshToken = Request.Cookies["refreshToken"];
//            if (!user.RefreshToken.Equals(refreshToken))
//            {
//                return (Unauthorized("Invalid Refresh Token"));
//            }
//            else if (user.TokenExpires < DateTime.Now)
//            {
//                return (Unauthorized("Token expired"));
//            }
//
//            string token = GenerateAccessToken(user);
//            var newRefreshToken = GenerateRefreshToken();
//            SetRefreshToken(newRefreshToken);
//
//            return Ok(token);
//        }






