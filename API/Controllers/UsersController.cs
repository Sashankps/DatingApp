using System.Security.Claims;
using API.Dtos;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseAPIController
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
        {
            var users = await _userRepo.GetUsersAsync();
            var usersToReturn = _mapper.Map<IEnumerable<MemberDTO>>(users);
            return Ok(usersToReturn);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDTO>> GetUser(string username)
        {
            var user = await _userRepo.GetUserByUsernameAsync(username);
            var userToReturn = _mapper.Map<MemberDTO>(user);
            return Ok(userToReturn);
        }

        [HttpPut("{username}")]
        public async Task<ActionResult> UpdateUser(string username, MemberUpdateDTO memberUpdateDto)
        {
            var user = await _userRepo.GetUserByUsernameAsync(username);
            if (user == null) return NotFound();
            _mapper.Map(memberUpdateDto, user);
            if (await _userRepo.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update user");
        }
    }
}