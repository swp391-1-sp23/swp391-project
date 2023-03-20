using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
 
using AutoMapper;
 
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Extensions;
 
using SWP391.Project.Entities;
using SWP391.Project.Models;
using SWP391.Project.Models.Dtos.Login;
using SWP391.Project.Models.Dtos.Register;
using SWP391.Project.Repositories;
 
namespace SWP391.Project.Services
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginInput input);
        Task<bool> RegisterAsync(RegisterInput input, AccountRole role = AccountRole.Customer);
    }
 
    public class AuthService : IAuthService
    {
        private readonly JWTModel _jwtModel;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
 
        public AuthService(IAccountRepository accountRepository,
                           IOptions<JWTModel> jwtModel,
                           IMapper mapper)
        {
            _jwtModel = jwtModel.Value;
            _accountRepository = accountRepository;
            _mapper = mapper;
        }
 
        public async Task<string?> LoginAsync(LoginInput input)
        {
            AccountEntity? account = await _accountRepository.GetSingleAsync(predicate: item => !item.IsDeleted && item.Email == input.Email);
 
            if (account == null)
            {
                return null;
            }
 
            bool passwordMatched = CheckPassword(providedPassword: input.Password, knownPassword: account.Password!, salt: account.Salt!);
 
            return !passwordMatched
                ? null
                : GenerateJWTToken(id: account.Id, email: account.Email, role: account.Role);
        }
 
        public async Task<bool> RegisterAsync(RegisterInput input, AccountRole role = AccountRole.Customer)
        {
            AccountEntity? existingAccount = await _accountRepository.GetByEmailAsync(email: input.Email);
 
            if (existingAccount != null && existingAccount.IsDeleted == false)
            {
                return false;
            }
 
            AccountEntity account = _mapper.Map(source: input, destination: existingAccount ?? new());
 
            HMACSHA512 hmac = new();
            account.Salt = hmac.Key;
            account.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(input.Password));
            // account.FirstName = input.FirstName;
            // account.LastName = input.LastName;
            // account.Phone = input.Phone;
            account.Role = role;
            account.IsDeleted = false;
 
            return existingAccount == null
                ? await _accountRepository.AddAsync(entity: account)
                : await _accountRepository.UpdateAsync(entity: account);
        }
 
        private static bool CheckPassword(string providedPassword, byte[] knownPassword, byte[] salt)
        {
            HMACSHA512 hmac = new(key: salt);
            byte[] compute = hmac.ComputeHash(Encoding.UTF8.GetBytes(providedPassword));
            return compute.SequenceEqual(knownPassword);
        }
 
        private string GenerateJWTToken(Guid id, string email, AccountRole role)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] secret = Encoding.ASCII.GetBytes(s: _jwtModel.Secret);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new(claims: new[]
                {
                    new Claim(type: ClaimTypes.NameIdentifier, value: id.ToString()),
                    new Claim(type: ClaimTypes.Email,value: email),
                    new Claim(type: ClaimTypes.Role,value: role.GetDisplayName())
                }),
 
                Expires = DateTime.UtcNow.AddDays(value: 7),
 
                SigningCredentials = new(
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
