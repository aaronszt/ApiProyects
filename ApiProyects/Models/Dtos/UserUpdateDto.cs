using System.ComponentModel.DataAnnotations;

namespace ApiProyects.Models.Dtos
{
    public class UserUpdateDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string ImageUrl { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
