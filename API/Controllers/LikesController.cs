using API.Dtos;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LikesController : BaseAPIController
    {
        private readonly ILikesRepository _likesRepo;
        private readonly IUserRepository _userRepo;

        public LikesController(IUserRepository userRepo, ILikesRepository likesRepo)
        {
            _likesRepo = likesRepo;
            _userRepo = userRepo;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = int.Parse(User.GetUserId());
            var likedUser = await _userRepo.GetUserByUsernameAsync(username);
            var sourceUser = await _likesRepo.GetUserWithLikes(sourceUserId);
            if (likedUser == null) return NotFound();
            if (sourceUser.UserName == username) return BadRequest("YOu cannot like yourself");

            var userLike = await _likesRepo.GetUserLike(sourceUserId, likedUser.Id);
            if (userLike != null) return BadRequest("You already like this user");

            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                TargetUserId = likedUser.Id
            };
            sourceUser.LikedUsers.Add(userLike);
            if (await _userRepo.SaveAllAsync()) return Ok();
            return BadRequest("Unable to like the user");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<LikeDTO>>> GetUserLikes([FromQuery] LikesParams likesParams)
        {
            likesParams.UserId = int.Parse(User.GetUserId());
            var users = await _likesRepo.GetUserLikes(likesParams);
            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
            return Ok(users);
        }

    }
}