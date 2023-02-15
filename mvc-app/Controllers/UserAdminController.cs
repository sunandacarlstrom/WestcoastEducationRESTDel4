using Microsoft.AspNetCore.Mvc;
using WestcoastEducation.Web.Interfaces;
using WestcoastEducation.Web.Models;
using WestcoastEducation.Web.ViewModels.Users;

namespace WestcoastEducation.Web.Controllers;

[Route("admin/user")]
public class UserAdminController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public UserAdminController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _unitOfWork.UserRepository.ListAllAsync();
        var users = result.Select(u => new UserListViewModel
        {
            UserId = u.UserId,
            UserName = u.UserName,
            Email = u.Email,
            FirstName = u.FirstName,
            LastName = u.LastName,
            SocialSecurityNumber = u.SocialSecurityNumber,
            StreetAddress = u.StreetAddress,
            PostalCode = u.PostalCode,
            Phone = u.Phone,
            IsATeacher = u.IsATeacher
        }).ToList();

        return View("Index", users);
    }

    [Route("details/{userId}")]
    public async Task<IActionResult> Details(int userId)
    {
        var result = await _unitOfWork.UserRepository.FindByIdAsync(userId);

        if (result is null) return View(nameof(Index));

        var user = new UserListViewModel()
        {
            UserId = result.UserId,
            UserName = result.UserName,
            Email = result.Email,
            FirstName = result.FirstName,
            LastName = result.LastName,
            SocialSecurityNumber = result.SocialSecurityNumber,
            StreetAddress = result.StreetAddress,
            PostalCode = result.PostalCode,
            Phone = result.Phone,
            IsATeacher = result.IsATeacher
        };

        // skicka användaren till vyn
        return View("Details", user);
    }

    [HttpGet("create/{isATeacher}")]
    public IActionResult Create(bool isATeacher)
    {
        // skapa ett nytt objekt för att skicka över till vyn
        var user = new UserPostViewModel();
        // stoppar in boolean värdet in i modellen User för att använda i vyn 
        user.IsATeacher = isATeacher;

        return View("Create", user);
    }

    [HttpPost("create/{isATeacher}")]
    public async Task<IActionResult> Create(UserPostViewModel user)
    {
        {
            // tittar på om det som kommer in stämmer överrens med kraven i UserPostViewModel
            if (!ModelState.IsValid) return View("Create", user);

            // skapar ett felmeddelande ifall E-postadressen redan finns
            if (await _unitOfWork.UserRepository.FindByEmailAsync(user.Email) is not null)
            {
                var error = new ErrorModel
                {
                    ErrorTitle = "Användaren finns redan!",
                    ErrorMessage = $"Användare med e-postadressen {user.Email} finns redan i systemet"
                };

                //skicka tillbaka en vy som visar information gällande felet 
                return View("_Error", error);
            }

            //UserModel (datamodell) är det jag kan skicka till databasen
            var userToAdd = new UserModel
            {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                SocialSecurityNumber = user.SocialSecurityNumber,
                StreetAddress = user.StreetAddress,
                PostalCode = user.PostalCode,
                Phone = user.Phone,
                IsATeacher = user.IsATeacher,
                Password = user.Password
            };

            // Om allt går bra, inga fel inträffar...

            // lägg upp användaren i minnet
            if (await _unitOfWork.UserRepository.AddAsync(userToAdd))
            {
                //spara ner det i databasen
                if (await _unitOfWork.Complete())
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            // annars inträffar ett fel som vi inte har räknat med
            return View("Error", new ErrorModel
            {
                ErrorTitle = "Kunde inte spara användaren",
                ErrorMessage = $"Ett fel har inträffat när användare {user.CompleteName} skulle sparas"
            });
        }
    }

    [HttpGet("edit/{userId}")]
    public async Task<IActionResult> Edit(int userId)
    {
        var result = await _unitOfWork.UserRepository.FindByIdAsync(userId);

        if (result is null)
        {
            return View("Error", new ErrorModel
            {
                ErrorTitle = "Kunde inte hitta användaren",
                ErrorMessage = $"Det gick inte att hitta en användare med id {userId}"
            });
        }

        var userToUpdate = new UserUpdateViewModel
        {
            UserId = result.UserId,
            UserName = result.UserName,
            Email = result.Email,
            FirstName = result.FirstName,
            LastName = result.LastName,
            SocialSecurityNumber = result.SocialSecurityNumber,
            StreetAddress = result.StreetAddress,
            PostalCode = result.PostalCode,
            Phone = result.Phone,
            IsATeacher = result.IsATeacher
        };

        return View("Edit", userToUpdate);
    }

    [HttpPost("edit/{userId}")]
    public async Task<IActionResult> Edit(int userId, UserUpdateViewModel user)
    {
        try
        {
            // kontrollerar att ModelState är giltigt, om inte så retunerar den tillbaka till vår vy Edit och skickar med objekten och ser vilka fel vi måste åtgärda
            if (!ModelState.IsValid) return View("Edit", user);

            //om vår modell är giltig då hämtar vi vår användare från databasen med Id som vi får in i vårt anrop 
            var userToUpdate = await _unitOfWork.UserRepository.FindByIdAsync(userId);

            //om jag inte fick den av någon orsak då skickar jag iväg en ny felmodell 
            if (userToUpdate is null)
            {
                var notFoundError = new ErrorModel
                {
                    ErrorTitle = "Användare saknas!",
                    ErrorMessage = $"Det gick inte att hitta användaren {user.CompleteName}"
                };

                return View("_Error", notFoundError);
            }

            //om användaren finns då ska vi uppdatera den användaren vi plockade ut från databasen 
            //endast det jag väljer att uppdateras här kommer att behövas uppdateras av admin
            userToUpdate.Email = user.Email;
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.SocialSecurityNumber = user.SocialSecurityNumber;
            userToUpdate.StreetAddress = user.StreetAddress;
            userToUpdate.PostalCode = user.PostalCode;
            userToUpdate.Phone = user.Phone;
            userToUpdate.IsATeacher = user.IsATeacher;

            //försök att göra en UpdateAsync för att spara i minnet 
            if (await _unitOfWork.UserRepository.UpdateAsync(userToUpdate))
            {
                //försöker spara till databasen 
                if (await _unitOfWork.Complete())
                {
                    //om sant så skickas man vidare till Index sidan 
                    return RedirectToAction(nameof(Index));
                }
            }

            return View("_Error", new ErrorModel
            {
                ErrorTitle = "Ett fel inträffade",
                ErrorMessage = "Något gick fel när användaren skulle uppdateras"
            });
        }

        catch (Exception ex)
        {
            return View("_Error", new ErrorModel
            {
                ErrorTitle = "Ett oväntat fel har inträffat",
                ErrorMessage = ex.Message
            });
        }
    }

    [Route("delete/{userId}")]
    public async Task<IActionResult> Delete(int userId)
    {
        try
        {
            //letar upp användaren som vi ska ta bort 
            var userToDelete = await _unitOfWork.UserRepository.FindByIdAsync(userId);

            // gör samma kontroll igen, om användaren inte fanns så retuneras Index istället för Error-sida 
            if (userToDelete is null) return RedirectToAction(nameof(Index));

            //annars gör ett försök att lägga användaren i Delete-kö i Changetracking och går det bra får man tillabka true
            if (await _unitOfWork.UserRepository.DeleteAsync(userToDelete))

            {
                //går det bra skicka alla ändringar till datrabasenm 
                if (await _unitOfWork.Complete())
                {
                    //tillsut gå tillbaka itill Index 
                    return RedirectToAction(nameof(Index));
                }
            }
            return View("_Error", new ErrorModel
            {
                ErrorTitle = "Ett fel inträffade när användaren skulle tas bort",
                ErrorMessage = $"Ett fel inträffade när användare med id {userId} skulle raderas"
            });
        }
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett oväntat fel har inträffat när användaren skulle raderas",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }
    }
}