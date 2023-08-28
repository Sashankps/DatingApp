using API.Data;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // [HttpDelete("{id}")]
        // public async Task<ActionResult<AppUser>> Delete(int id)
        // {
        //     var user = await _db.Users.SingleOrDefaultAsync(x => x.Id == id);
        //     if (user == null) return BadRequest();

        //     _db.Users.Remove(user);
        //     await _db.SaveChangesAsync();
        //     return Ok(user);
        // }
    }
}