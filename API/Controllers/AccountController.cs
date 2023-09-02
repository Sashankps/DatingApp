using System.Security.Cryptography;
using System.Text;

using API.Data;
using API.Dtos;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly AppDbContext _db;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(AppDbContext db, ITokenService tokenService, IMapper mapper)
        {
            _db = db;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")] //api/Account/register
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            var user = _mapper.Map<AppUser>(registerDto);
            //using a inbuilt hashing algorithm from Asp.Net
            //using - to dispose hmac af    ter it becomes redundant through garbage collection 
            using var hmac = new HMACSHA512();


            user.UserName = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;


            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return new UserDTO
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {
            var user = await _db.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);
            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return new UserDTO
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _db.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}