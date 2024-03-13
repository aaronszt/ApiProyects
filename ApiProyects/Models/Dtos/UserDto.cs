using System.ComponentModel.DataAnnotations;

namespace ApiProyects.Models.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}
