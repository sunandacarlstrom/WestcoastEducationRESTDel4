using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.Web.Models;

public class UserModel
{
    [Key]
    public int UserId { get; set; }
    public string UserName { get; set; } = "";
    public string Email { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string CompleteName { get { return FirstName + " " + LastName; } }
    public string SocialSecurityNumber { get; set; } = "";
    public string StreetAddress { get; set; } = "";
    public string PostalCode { get; set; } = "";
    public string Phone { get; set; } = "";
    public bool IsATeacher { get; set; } = false;
    public string Password { get; set; } = "";
}