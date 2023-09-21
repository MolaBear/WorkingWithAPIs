using System.ComponentModel.DataAnnotations;

namespace WorkingWithAPIs.Models
{
    public class UsersModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string firstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        public string lastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        public string email { get; set; }

        [Required(ErrorMessage = "Telephone is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(15, ErrorMessage = "Telephone number cannot be longer than 15 characters.")]
        public string telephone { get; set; }

        [Required(ErrorMessage = "Identity number is required.")]
        [StringLength(20, ErrorMessage = "Identity number cannot be longer than 20 characters.")]
        public string identityNumber { get; set; }
    }
}
