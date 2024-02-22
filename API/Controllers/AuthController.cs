using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
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
public class AuthController(UserManager<ApplicationUser> _userManager , RoleManager<IdentityRole> _roleManager , IConfiguration configuration, IHttpClientFactory _httpClientFactory) : ControllerBase
{

    private readonly UserManager<ApplicationUser> _userManager = _userManager;
    private readonly RoleManager<IdentityRole> _roleManager = _roleManager;
    private readonly IConfiguration configuration  = configuration;
    private readonly IHttpClientFactory _httpClientFactory = _httpClientFactory; 





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
        if (newUser != null) return Ok(new GeneralResponseB(401, "Conflict"));



        var createUser = await _userManager.CreateAsync(userNew!, registerResponse.Password);
        if (!createUser.Succeeded) return Ok(new GeneralResponseB(400, "Failed Creating User .. try agian"));

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
        return await LoginHelper(foundUser);
    }



















    [HttpPost] 
    public async Task<IActionResult> Login([FromBody] LoginResponse loginResponse)
    {

        //var userStores = await _userStore.Users.ToListAsync<IdentityUser>();

        var foundUser = await _userManager.FindByEmailAsync(loginResponse.Email);
        var comparePassword = await _userManager.CheckPasswordAsync(foundUser, loginResponse.Password);
        if (!comparePassword || foundUser is null) return Ok(new GeneralResponseB(405, "Email/Password incorrect"));

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
            PasswordHash = googleAuthValidate.Email,
            UserName = googleAuthValidate.Email,
            PhoneNumberConfirmed = true,
            PhotoUrl = googleAuthValidate.Picture
            //PhoneNumber = googleAuthValidate.PhoneNumber,
            //CurrentLocationId = googleAuthValidate.CurrentLocationId,
            //ProfilePhotoId = googleAuthValidate.ProfilePhotoId,
        };



        var newUser = await _userManager.FindByEmailAsync(userNew.Email);
        if (newUser != null)
        {
            var klando = await _userManager.GetAuthenticationTokenAsync(newUser, "Klando", "refresh");
            var google = await _userManager.GetAuthenticationTokenAsync(newUser, "Google", "refresh");

            if (google == null && (klando != null)) return Ok(new GeneralResponse(false, "Email is Taken / try signing in with klando or facebook"));
            return await LoginHelper(newUser, "Google");
        }
        var createUser = await _userManager.CreateAsync(userNew!, (userNew.PasswordHash + Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)).ToString()));
        if (!createUser.Succeeded) return Ok(new GeneralResponseB(400, "Failed Creating User cuz you already exist . "));

        var userList = await _userManager.Users.ToListAsync<ApplicationUser>();
        var userPresent = userList.Count != 1;
        if (userPresent == false)
        {
            await _userManager.AddToRoleAsync(userNew!, "Admin");
        }
        else
        {
            var user = await _roleManager.FindByNameAsync("User");
            if (user is null) await _roleManager.CreateAsync(new IdentityRole { Name = "User" });
            await _userManager.AddToRoleAsync(userNew!, "User");
          

        }

        var foundUser = await _userManager.FindByEmailAsync(userNew.Email);

        return await LoginHelper(foundUser, "Google");

    }






    [HttpGet]
    public async Task<IActionResult> UserInfo(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        return Ok(user);
    }


    [HttpPost]
    public async Task<IActionResult> GetUserRole([FromBody] ApplicationUser applicationUser)
    {
        Log.Information($"app User => {applicationUser}");
        var roleobj = await _userManager.GetRolesAsync(applicationUser);
        return Ok(roleobj);
    }












    [HttpPost]
    public async Task<IActionResult> FaceBookAuth([FromBody] FacebookRequestType facebookRequestType)
    {
        // facebookRequestType.AppId = configuration["Authentication:Facebook:AppId"];


        var facebookResponse = await ValidateFacebookToken(facebookRequestType);


        Log.Information("facebookResponse => {@facebookResponse}", facebookResponse);


        if (facebookResponse is null) return Ok(new GeneralResponse(false, "Not Validated By FaceBook"));

        var userNew = new ApplicationUser()
        {
            Name = facebookRequestType.Name,
            Email = facebookRequestType.Email,
            PasswordHash = facebookRequestType.Email,
            UserName = facebookRequestType.Email,
            PhoneNumberConfirmed = true,
            PhotoUrl = facebookRequestType.PhotoUrl
            //PhoneNumber = googleAuthValidate.PhoneNumber,
            //CurrentLocationId = googleAuthValidate.CurrentLocationId,
            //ProfilePhotoId = googleAuthValidate.ProfilePhotoId,
        };
        var newUser = await _userManager.FindByEmailAsync(userNew.Email);
        if (newUser != null)
        {
            var klando = await _userManager.GetAuthenticationTokenAsync(newUser, "Klando", "refresh");
            var facebook = await _userManager.GetAuthenticationTokenAsync(newUser, "Facebook", "refresh");
            if (facebook == null && klando != null ) return Ok(new GeneralResponse(false, "Email is Taken / try signing in with klando or google"));
            return await LoginHelper(newUser, "Facebook");
        }
        var createUser = await _userManager.CreateAsync(userNew!, (userNew.PasswordHash + Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)).ToString()));
        if (!createUser.Succeeded) return Ok(new GeneralResponseB(400, "Failed Creating User cuz you already exist . "));
        var userList = await _userManager.Users.ToListAsync<ApplicationUser>();
        var userPresent = userList.Count != 1;
        if (userPresent == false)
        {
            await _userManager.AddToRoleAsync(userNew!, "Admin");
        }
        else
        {
            await _userManager.AddToRoleAsync(userNew!, "User");
        }
        var foundUser = await _userManager.FindByEmailAsync(userNew.Email);

        return await LoginHelper(foundUser, "Facebook");

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





    private async Task<FacebookTokenValidationResult> ValidateFacebookToken(FacebookRequestType facebookRequestType)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var appAccessTokenResponse = await httpClient.GetFromJsonAsync<FacebookAppAccessTokenResponse>($"https://graph.facebook.com/oauth/access_token?client_id={configuration["Authentication:Facebook:ClientId"]}&client_secret={configuration["Authentication:Facebook:ClientSecret"]}&grant_type=client_credentials");

        var response =
            await httpClient.GetFromJsonAsync<FacebookTokenValidationResult>(
                $"https://graph.facebook.com/debug_token?input_token={facebookRequestType.AccessToken}&access_token={appAccessTokenResponse!.AccessToken}");

        if (response is null || !response.Data.IsValid)
        {
            return null; 
           // return Result.Fail($"{request.Provider} access token is not valid.");
        }

        return response;
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
        var refreshToken = GenerateRefreshToken(foundUser);
        await _userManager.SetAuthenticationTokenAsync(foundUser, tokenProvider, "refresh", refreshToken);
        var jwttoken = GenerateAccessToken(foundUser);
        Log.Information($"------------LLLLL => ----------{refreshToken}-------------");
        return Ok(new LoginResponseithTokens(jwttoken, 1, refreshToken, DateTime.Now));
    }








    [HttpPost]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenResponse _refreshToken)
    {

        var refreshToken = _refreshToken.Token;
        Log.Information($"------------LLLLL => ------------{refreshToken}-----------------------------------");
        if (refreshToken is null) return Ok("No refresh token ");
        var tokenObj = new JwtSecurityToken(refreshToken);
        var email = tokenObj.Claims.First().Value;
        var foundUser = await _userManager.FindByEmailAsync(email);

        if (email == null || foundUser == null) return Ok(new GeneralResponse(false, "User Does not exist"));

       // string getRefreshToken = await _userManager.GetAuthenticationTokenAsync(foundUser, "Klando", "refresh") ??
       //     await _userManager.GetAuthenticationTokenAsync(foundUser, "Facebook", "refresh") ??
       //     await _userManager.GetAuthenticationTokenAsync(foundUser, "Google", "refresh");

        string getRefreshToken = "";
        string tokenProvider = "";

        if (await _userManager.GetAuthenticationTokenAsync(foundUser, "Klando", "refresh") is not null)
        {
            getRefreshToken =await _userManager.GetAuthenticationTokenAsync(foundUser, "Klando", "refresh");
            tokenProvider = "Klando";

        }
        else if(await _userManager.GetAuthenticationTokenAsync(foundUser, "Facebook", "refresh") is not null)
        {
            getRefreshToken = await _userManager.GetAuthenticationTokenAsync(foundUser, "Facebook", "refresh");
            tokenProvider = "Facebook";

        }
        else if(await _userManager.GetAuthenticationTokenAsync(foundUser, "Google", "refresh") is not null)
        {
            getRefreshToken = await _userManager.GetAuthenticationTokenAsync(foundUser, "Google", "refresh");
            tokenProvider = "Google";

        }


        //var tokenprovider = await _userManager.tok






        Log.Information($"------------refresh => ------------{refreshToken}-----------------------------------");
        Log.Information($"------------getRefresh => ------------{getRefreshToken}-----------------------------------");



        if (getRefreshToken != refreshToken) return Ok(new GeneralResponse(false, "Invalid Request"));


        return await LoginHelper(foundUser , tokenProvider);
    }

}

