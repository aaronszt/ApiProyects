using ApiProyects.Files;
using ApiProyects.Models;
using ApiProyects.Models.Dtos;
using ApiProyects.Validators;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiProyects.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper;
        public UserController(ApplicationDBContext db, IMapper mapper)
        {
            _db = db; 
            _mapper = mapper; 
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            IEnumerable<User> userList = await _db.Users.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<UserDto>>(userList)); 
        }

        [HttpGet("id", Name = "GetUserById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserDto>>GetUserById(int id)
        {
            if (id == 0) return BadRequest();

            var user = await _db.Users.FirstOrDefaultAsync(i => i.Id == id);
            if (user == null) return NotFound();

            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserCreateDto userCreateDto)
        {
            UserCreateDtoValidator validator = new UserCreateDtoValidator();
            ValidationResult result = validator.Validate(userCreateDto);
            if (!result.IsValid) return BadRequest(result.Errors);

            User userModel = _mapper.Map<User>(userCreateDto);
            userModel.CreationDate = DateTime.Now;

            await _db.Users.AddAsync(userModel);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetUserById", userModel);
        }

        [HttpDelete ("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (id <= 0) return BadRequest();

            var user = await _db.Users.FirstOrDefaultAsync(i => i.Id == id);
            if (user == null) return NotFound();

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateUser (int id, [FromBody] UserDto userDto)
        {
            if (userDto == null || id != userDto.Id) return BadRequest();

            User userModel = _mapper.Map<User>(userDto);
            userModel.UpdateDate = DateTime.Now;

            _db.Users.Update(userModel);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdatePartialUser(int id, JsonPatchDocument<UserDto> patchDto)
        {
            if (patchDto == null || id == 0) return BadRequest();

            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
            if (user == null) return NotFound();

            UserDto userDto = _mapper.Map<UserDto>(user);

            patchDto.ApplyTo(userDto, ModelState);
            if (!ModelState.IsValid) return BadRequest();

            User userModel = _mapper.Map<User>(userDto);

            userModel.UpdateDate = DateTime.Now;

            _db.Users.Update(userModel);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
