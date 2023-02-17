namespace WestcoastEducation.Web.ViewModels.TeacherSkills
{
    public class TeacherSkillsListViewModel
    {
        public int Id { get; set; }
        
        //P.S. stämmer överrens med namngivningen av egenskapen i mitt api, men bör egentligen heta Namn (eftersom det är namnet på en Skill)
        public string Skill { get; set; }
    }
}