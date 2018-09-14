using System.ComponentModel.DataAnnotations;

namespace WebApiTestingSkeleton.API.DTO
{
    public class UpserUserDto
    {
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Fullname { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
