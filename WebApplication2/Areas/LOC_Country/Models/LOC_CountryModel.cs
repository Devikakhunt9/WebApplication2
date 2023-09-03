using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Areas.LOC_Country.Models
{
    public class LOC_CountryModel
    {
        public int CountryID { get; set; }
        [Required]
        public string? CountryName { get; set; }
        [Required]
        public string? CountryCode { get; set;}
    }

    public class CountrySearchModel 
    {
        public string?  CountryName { get; set; }
        public string? CountryCode { get; set; }
    }

}


