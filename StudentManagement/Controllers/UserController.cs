using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentManagement.Modals;
using StudentManagement.Modals.Request;
using StudentManagement.Modals.Response;
using StudentManagement.Utils;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentManagement.ControllersBase
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly DB _context;
        public UserController(DB context)
        {
            _context = context;
            if (_context.Users.ToList().Count() == 0)
            {
                User newUser = new User
                {
                    UserName = "admin",
                    Password = Utils.Helper.GenHash("123456"),
                    FullName = "System admin"
                };
                _context.Users.Add(newUser);
                _context.SaveChanges();
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(long id)
        {
            return await _context.Users.FindAsync(id);
        }

        [HttpPost("login")]
        public async Task<ActionResult<BaseResponse>> Login(LoginRequest request)
        {
            if (!String.IsNullOrEmpty(request.UserName)&&!String.IsNullOrEmpty(request.Password))
            {
                var user = await _context.Users
                            .Where(x => x.UserName == request.UserName &&
                            x.Password == Utils.Helper.GenHash(request.Password))
                            .AsNoTracking()
                            .SingleOrDefaultAsync();
                if (user != null)
                {
                    //generate token
                    var claimData = new[] { new Claim(ClaimTypes.Name, request.UserName) };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Helper.AppKey));
                    var signingCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: Helper.Issuer,
                        audience: Helper.Issuer,
                        expires: DateTime.Now.AddMinutes(30),
                        claims: claimData,
                        signingCredentials: signingCredential
                    );
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                    return new BaseResponse(new LoginResponse
                    {
                        UserId = user.UserId,
                        UserName = user.UserName,
                        Token = "Bearer " + tokenString
                    });
                }
                else
                {
                    return new BaseResponse
                    {
                        ErrorCode = 1,
                        Message = "Wrong username or password!"
                    };
                }
               
            }

            return new BaseResponse
            {
                ErrorCode = 2,
                Message = "Missing fields!"
            };
        }
        
    }
}
