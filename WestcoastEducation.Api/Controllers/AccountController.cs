using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WestcoastEducation.Api.Models;
using WestcoastEducation.Api.Services;
using WestcoastEducation.Api.ViewModels.Account;

namespace WestcoastEducation.Api.Controllers;

[ApiController]
[Route("api/v1/account")]
public class AccountController : ControllerBase
{
    private readonly UserManager<UserModel> _userManager;
    private readonly TokenService _tokenService;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public AccountController(UserManager<UserModel> userManager, TokenService tokenService)
    {
        _tokenService = tokenService;
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterViewModel model)
    {

        //objektet som ska registreras och läggas till i systemet 
        var user = new UserModel
        {
            UserName = model.UserName,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        // skapar användaren och sparar ner direkt till databasen 
        var result = await _userManager.CreateAsync(user, model.Password);

        // tittar på listan av fel som genereas av Identity biblioteket 
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            // returnerar alla meddelanden som jag får in i ModelState 
            return ValidationProblem();
        }

        // ger användaren en standardroll som "User" då detta är en publik hantering där användararna själva registrerar sig i systemet
        // övriga roller såsom "Student" & "Teacher" görs under admin-verktyg
        await _userManager.AddToRoleAsync(user, "User");
        // vill inte automatiskt logga in användaren, endast retunera ett OK! 
        return StatusCode(201);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
        // hitta en användare i systemet 
        var user = await _userManager.FindByNameAsync(model.UserName);
        // om användaren inte existerar eller inte anger rätt lösenord då retuneras ett felmeddelande med ".Unauthorized" 
        if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            return Unauthorized();
        }

        //retunerar en vymodell för användaren som även skapar ett nytt Token 
        return Ok(new UserViewModel
        {
            Email = user.Email,
            Token = await _tokenService.CreateToken(user)
        });
    }
}
