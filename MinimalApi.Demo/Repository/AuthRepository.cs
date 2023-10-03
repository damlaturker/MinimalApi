﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MinimalApi.Demo.Data;
using MinimalApi.Demo.Models;
using MinimalApi.Demo.Models.DTO;
using MinimalApi.Demo.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MinimalApi.Demo.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string secretKey;
        public AuthRepository(ApplicationDbContext db, IMapper mapper, IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _mapper = mapper;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            secretKey = _configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.UserName == username);

            // return null if user not found
            if (user == null)
                return true;

            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.SingleOrDefault(x => x.UserName == loginRequestDto.UserName);
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);


            //user not found
            if (user == null || isValid == false)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.Role,roles.FirstOrDefault()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDto loginResponseDto = new()
            {
                User = _mapper.Map<UserDto>(user),
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
            return loginResponseDto;
        }

            public async Task<UserDto> Register(RegistrationRequestDto requestDto)
            {
            ApplicationUser userObj = new()
            {
                UserName = requestDto.UserName,
                Name = requestDto.Name,
                NormalizedEmail = requestDto.UserName.ToUpper(),
                Email = requestDto.UserName,
            };

            try
            {
                var result = await _userManager.CreateAsync(userObj, requestDto.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("customer"));
                    }
                    await _userManager.AddToRoleAsync(userObj, "admin");

                    var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == requestDto.UserName);
                    return _mapper.Map<UserDto>(user);
                }
            }
            catch (Exception e)
            {

            }
            return null;
        }
    }
}
