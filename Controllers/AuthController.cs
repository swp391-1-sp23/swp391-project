using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using SWP391.Project.Entities;
using SWP391.Project.Models;
using SWP391.Project.Models.Dtos.Login;
using SWP391.Project.Models.Dtos.Register;
using SWP391.Project.Repositories;

namespace SWP391.Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JWTModel _jwtModel;
        private readonly IAccountRepository _accountRepository;

        public AuthController(IOptions<JWTModel> jwtModel, IAccountRepository accountRepository)
        {
            _jwtModel = jwtModel.Value;
            _accountRepository = accountRepository;
        }

        [HttpPost(template: "login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginInput input)
        {
            var account = await _accountRepository.GetAccountByEmail(input.Email);
            if (account == null)
            {
                return BadRequest(error: "ACCOUNT.VALIDATE.INVALID");
            }

            var passwordMatched = CheckPassword(input: input.Password, account: account);

            return passwordMatched ? GenerateToken(account: account) : BadRequest(error: "ACCOUNT.CREDENTIAL.INVALID");
        }


        [HttpPost(template: "register")]
        public async Task<ActionResult<bool>> Register([FromBody] RegisterInput input)
        {
            var account = new AccountEntity
            {
                Email = input.Email,
                Role = input.Role
            };

            HMACSHA512? hmac = new HMACSHA512();
            account.Salt = hmac.Key;
            account.Password = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input.Password));

            return await _accountRepository.AddAccount(account: account);
        }

        [HttpGet(template: "check")]
        [Authorize(Roles = "string")]
        public ActionResult<bool> Check()
        {
            return true;
        }

        private bool CheckPassword(string input, AccountEntity account)
        {
            HMACSHA512 hmac = new HMACSHA512(account.Salt);
            var compute = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
            return compute.SequenceEqual(account.Password);
        }

        private string GenerateToken(AccountEntity account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = Encoding.ASCII.GetBytes(s: _jwtModel.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims: new[]
                {
                    new Claim(type: ClaimTypes.Email,value: account.Email),
                    new Claim(type: ClaimTypes.Role,value: account.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(value: 7),
                SigningCredentials = new SigningCredentials(
                    key: new SymmetricSecurityKey(secret),
                    algorithm: SecurityAlgorithms.HmacSha512Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var serializedToken = tokenHandler.WriteToken(token);

            return serializedToken;
        }
    }
}