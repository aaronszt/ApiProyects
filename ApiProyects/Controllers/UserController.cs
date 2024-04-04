using ApiProyects.Files;
using ApiProyects.Models;
using ApiProyects.Models.Dtos;
using ApiProyects.Repository.IRepository;
using ApiProyects.Validators;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace ApiProyects.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        protected ApiResponse _response;
        public UserController(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ApiResponse>> GetUsers()
        {
            try
            {
                IEnumerable<User> userList = await _userRepo.GetAll();
                _response.Result = _mapper.Map<IEnumerable<UserDto>>(userList);
                _response.statusCode = HttpStatusCode.OK;
                return Ok(_response); 
            }
            catch (Exception ex)
            {
                _response.IsSuccessfull = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("id", Name = "GetUserById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ApiResponse>>GetUserById(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "sadasdasdsadsada" };
                    _response.IsSuccessfull = false;
                    return BadRequest(_response);
                }

                var user = await _userRepo.GetOne(i => i.Id == id);
                if (user == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "aaa" };
                    _response.IsSuccessfull = false;
                    return NotFound(_response);
                }

                _response.statusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<UserDto>(user);

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessfull = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ApiResponse>> CreateUser([FromBody] UserCreateDto userCreateDto)
        {
            try
            {
                UserCreateDtoValidator validator = new UserCreateDtoValidator();
                ValidationResult result = validator.Validate(userCreateDto);
                if (!result.IsValid)
                {
                    var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccessfull = false;
                    _response.ErrorMessages = errorMessages;
                    return BadRequest(_response);        
                };

                User userModel = _mapper.Map<User>(userCreateDto);
                userModel.CreationDate = DateTime.Now;

                await _userRepo.Create(userModel);

                _response.Result = userModel;
                _response.statusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetUserById", _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessfull = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete ("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "message error" };
                    _response.IsSuccessfull = false;
                    return BadRequest(_response);
                }

                var user = await _userRepo.GetOne(i => i.Id == id);
                if (user == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "message error" };
                    _response.IsSuccessfull = false;
                    return NotFound(_response);
                }

                await _userRepo.Delete(user);

                _response.statusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessfull = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return BadRequest(_response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateUser (int id, [FromBody] UserDto userDto)
        {
            try
            {
                if (userDto == null || id != userDto.Id)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "error message" };
                    _response.IsSuccessfull = false;
                    return BadRequest(_response);
                }

                User userModel = _mapper.Map<User>(userDto);
                userModel.UpdateDate = DateTime.Now;

                await _userRepo.Update(userModel);

                _response.statusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessfull = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return BadRequest(_response);
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdatePartialUser(int id, JsonPatchDocument<UserDto> patchDto)
        {
            try
            {
                if (patchDto == null || id == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "error message" };
                    _response.IsSuccessfull = false;
                    return BadRequest();
                }

                var user = await _userRepo.GetOne(i => i.Id == id, tracked:false);
                if (user == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "error message" };
                    _response.IsSuccessfull = false;
                    return NotFound(_response);
                }

                UserDto userDto = _mapper.Map<UserDto>(user);

                patchDto.ApplyTo(userDto, ModelState);
                if (!ModelState.IsValid)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "error message" };
                    _response.IsSuccessfull = false;
                    return BadRequest(_response);
                }

                User userModel = _mapper.Map<User>(userDto);

                userModel.UpdateDate = DateTime.Now;

                await _userRepo.Update(userModel);

                _response.statusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessfull = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return BadRequest(_response);
        }
    }
}
