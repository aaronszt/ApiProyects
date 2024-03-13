using ApiProyects.Files;
using ApiProyects.Models;
using ApiProyects.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProyects.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<UserDto> GetUser()
        {
            return UserStore.usersList;
        }
        [HttpGet("id", Name = "GetUserById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<UserDto>GetUserById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var user = UserStore.usersList.FirstOrDefault(i => i.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<UserDto> CreateUser([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (userDto == null) return BadRequest();

            if (userDto.Id > 0) return StatusCode(StatusCodes.Status500InternalServerError);
            userDto.Id = UserStore.usersList.OrderByDescending(i => i.Id).FirstOrDefault().Id +1;
            UserStore.usersList.Add(userDto);

            return CreatedAtRoute("GetUserById", new { id = userDto.Id}, userDto);
        }

        [HttpDelete]
        public ActionResult<UserDto> DeleteUser(int id)
        {

        }
    }
}
