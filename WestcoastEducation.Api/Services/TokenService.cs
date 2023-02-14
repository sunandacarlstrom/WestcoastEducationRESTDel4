using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WestcoastEducation.Api.Models;

namespace WestcoastEducation.Api.Services;
public class TokenService
{
    private readonly UserManager<UserModel> _userManager;
    private readonly IConfiguration _config;
    public TokenService(UserManager<UserModel> userManager, IConfiguration config)
    {
        _config = config;
        _userManager = userManager;
    }

    // retunerar en string eftersom biljetten ska vara en sträng 
    public async Task<string> CreateToken(UserModel user)
    {
        // Här börjar JWT Payload... dvs. data som vi vill skicka in (paketera in i vår biljett)

        // Claim standardiserat sätt att paketera saker som användaren påstår sig vara 
        var claims = new List<Claim>{
                //skapar flera Claims (påståeende) i form av en lista
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName)
            };
        // titta vilka roller som en användare är registrerad på 
        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Här börjar Signature... dvs. krypteringen och hru vi ska kryptera det 

        // Jag vill skapa en ny symmetrisk nyckel genom att använda min hemliga nyckel som finns under inställningar i min appsettings.Development under egenskapen tokenSettings. Och gör om detta till Bytes enligt UTF8 standarden. 
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["tokenSettings:tokenKey"]));
        // gör en signering och krypetera den enligt denna algoritmen ".HmacSha512"
        var credientials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        //sätter upp inställningar för JWT SecurityToken 
        var options = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.Now.AddDays(5),
            signingCredentials: credientials
        );

        // få tillbaka JWT SecurityToken som en sträng
        return new JwtSecurityTokenHandler().WriteToken(options);
    }
}
