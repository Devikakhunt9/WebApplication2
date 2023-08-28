using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Areas.MST_Branch.Models
{
    public class MST_BranchModel
    {
        public int BranchID { get; set; }
        [Required]
        public string? BranchName { get; set; }
        [Required]
        public string? BranchCode { get; set;}
        public string? Created { get; set;}
        public string? Modified { get; set;}
    }
}
