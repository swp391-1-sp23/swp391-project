using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using SWP391.Project.Entities;
using SWP391.Project.Models;
using SWP391.Project.Models.Dtos.Login;
using SWP391.Project.Repositories;

namespace SWP391.Project.Services
{
    public interface IAuthService
    {
        Task<LoginOutput?> Login(string email, string password);
        Task<bool> Register(string email, string password, string role);
    }

    public class AuthService : IAuthService
    {
        private readonly JWTModel _jwtModel;
        private readonly IAccountRepository _accountRepository;

        public AuthService(IAccountRepository accountRepository, IOptions<JWTModel> jwtModel)
        {
            _jwtModel = jwtModel.Value;
            _accountRepository = accountRepository;
        }

        public async Task<LoginOutput?> Login(string email, string password)
        {
            var account = await _accountRepository.GetAccountByEmail(accountEmail: email);

            if (account == null) return null;

            var passwordMatched = CheckPassword(providedPassword: password, knownPassword: account.Password, salt: account.Salt);

            return !passwordMatched ? null : new LoginOutput()
            {
                Id = account.Id,
                Email = account.Email,
                Role = account.Role,
                Token = GenerateToken(email: account.Email, role: account.Role)
            };
        }

        public async Task<bool> Register(string email, string password, string role)
        {
            var accountExisted = await _accountRepository.GetAccountByEmail(accountEmail: email);

            if (accountExisted != null) return false;

            var account = new AccountEntity()
            {
                Email = email,
                Role = role,
            };

            HMACSHA512? hmac = new();
            account.Salt = hmac.Key;
            account.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return await _accountRepository.AddAccount(account: account);
        }

        private static bool CheckPassword(string providedPassword, byte[] knownPassword, byte[] salt)
        {
            HMACSHA512 hmac = new(salt);
            byte[] compute = hmac.ComputeHash(Encoding.UTF8.GetBytes(providedPassword));
            return compute.SequenceEqual(knownPassword);
        }

        private string GenerateToken(string email, string role)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] secret = Encoding.ASCII.GetBytes(s: _jwtModel.Secret);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims: new[]
                {
                    new Claim(type: ClaimTypes.Email,value: email),
                    new Claim(type: ClaimTypes.Role,value: role)
                }),
                Expires = DateTime.UtcNow.AddDays(value: 7),
                SigningCredentials = new SigningCredentials(
                    key: new SymmetricSecurityKey(secret),
                    algorithm: SecurityAlgorithms.HmacSha512Signature
                )
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string serializedToken = tokenHandler.WriteToken(token);

            return serializedToken;
        }
    }
}