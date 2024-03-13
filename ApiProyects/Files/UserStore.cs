using ApiProyects.Models.Dtos;

namespace ApiProyects.Files
{
    public static class UserStore
    {
        public static List<UserDto> usersList = new List<UserDto>
        {
            new UserDto
            {
                    Id = 1,
                    Email = "aaaa",
                    Name = "Test2",
            },
            new UserDto
            {
                    Id = 2,
                    Email = "aaaa",
                    Name = "Test2",
            },
            new UserDto
            {
                    Id = 3,
                    Email = "baaaa",
                    Name = "Test3",
            }
        };
    }
}
