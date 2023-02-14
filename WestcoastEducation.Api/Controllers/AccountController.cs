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
    public AccountController(UserManager<UserModel> userManager, TokenService tokenService)
    {
        _tokenService = tokenService;
        _userManager = userManager;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
        // hitta en användare i systemet 
        var user = await _userManager.FindByNameAsync(model.UserName);
        // om användaren inte existserar eller inte anger rätt lösenord då retuneras ett felmeddelande med ".Unauthorized" 
        if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            return Unauthorized();
        }

        //retunerar en vymodell för användaren som även skapar ett nytt Token 
        return Ok(new UserViewModel {
            Email = user.Email, 
            Token = await _tokenService.CreateToken(user)
        });
    }
}
