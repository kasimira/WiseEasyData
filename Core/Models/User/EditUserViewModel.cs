using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace Core.Models.User
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        public string LastName { get; set; }

        public string UserName { get; set; }

        public IEnumerable<SelectListItem>? Roles { get; set; }

        public List<string> RoleNames { get; set; } = new List<string>();
    }
}
