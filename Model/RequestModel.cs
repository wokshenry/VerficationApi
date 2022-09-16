using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace VerficationApi.Model
{
    public class RequestModel
    {
        [Required]
        [Display(Name = "Reference Id")]
        public string? referenceId { get; set; }
        [Required]
        [Display(Name = "National Identification Number")]
        public string? nin { get; set; }
        [Required]
        [StringLength(12, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 12)]
        [Display(Name = "Phone Number")]
        public string? phoneNumber { get; set; }
    }
}
