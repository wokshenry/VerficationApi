using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace VerficationApi.Model
{
    public class ListRequestModel
    {
        [Required]
        [Display(Name = "Statuses")]
        public string? Statuses { get; set; }
        //[Required]
        [Display(Name = "Genders")]
        public string? Genders { get; set; }
        [Required]
        [Display(Name = "From Date")]
        public DateTime? FromDate { get; set; }
        [Required]
        [Display(Name = "To Date")]
        public DateTime? ToDate { get; set; }
        [Required]
        [Display(Name = "Page Number")]
        public int? PageNumber { get; set; }
        [Required]
        [Display(Name = "Page Size")]
        public int? PageSize { get; set; }
    }
}
