using AutoMapper;
using MarketplaceAPI.DTOs;
using MarketplaceAPI.Entity;
using MarketplaceAPI.Interfaces;
using MarketplaceAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(UserManager<User> userManager, ITokenService tokenService, IMapper mapper )
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (UserExist(registerDto.UserName)) return BadRequest("user exist");

            var user = _mapper.Map<User>(registerDto);

            user.UserName = registerDto.UserName.ToLower();

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) BadRequest(result.Errors);

            return new UserDto
            {
                UserName = registerDto.UserName,
                token = await _tokenService.CreateToken(user)
            };

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users
                       .SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);
            if (user == null) return Unauthorized("Invalid UserName");

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result) return Unauthorized("unvalid Password");

            return new UserDto
            {
                UserName = user.UserName,
                token = await _tokenService.CreateToken(user)
            };
        }
        private bool UserExist(string userName)
        {
            var userExist = _userManager.Users.Any(x => x.UserName == userName.ToLower());
            return userExist;
        }
    }
}
