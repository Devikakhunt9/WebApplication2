using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Areas.LOC_City.Models
{
    public class LOC_CityModel
    {
        public int CityID { get; set; }
        [Required]
        public string? CityName { get; set; }
        [Required]
        public string? CityCode { get; set;}
        [Required]
        public int StateID { get; set;}
        [Required]
        public int CountryID { get; set;}
        public string? StateName { get; set; }  
        public string? CountryName { get; set;}
    }
}
