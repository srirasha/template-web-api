using System.ComponentModel.DataAnnotations;

namespace Web.MyAPI.Models.Users
{
    public class CreateUserRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }
    }
}