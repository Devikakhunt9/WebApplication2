using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Areas.MST_Student.Models
{
    public class MST_StudentModel
    {
        [Required]
        public int StudentID { get;set; }
        [Required]
        public string? StudentName { get;set;}
        [Required]
        public int BranchID { get;set; }
        [Required]
        public int CityID { get;set; }
        [Required]
        public string? Email { get;set;}
        [Required]
        public string? MobileNoStudent { get;set;}
        [Required]
        public string? MobileNoFather { get;set;}
        [Required]
        public string? Address { get;set;}
        [Required]
        public DateTime BirthDate { get;set;}
        [Required]
        public bool IsActive { get;set;}
        [Required]
        public string? Gender { get;set;}
        [Required]
        public string? Password { get;set;}
        [Required]
        public string? Created { get;set;}
        public string? Modified { get;set;}
    }


    public class SearchModelStudent
    {
        public string? StudentName { get;set;}
        public string? BranchtName { get;set;}
        public string? CityName { get;set;}
    }
}
