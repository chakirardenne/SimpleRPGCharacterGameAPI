using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Tutorial_DotNet.Data;

public class AuthRepository : IAuthRepository {
    private readonly DatabaseContext _context;
    private readonly IConfiguration _configuration;

    public AuthRepository(DatabaseContext context, IConfiguration configuration) {
        _context = context;
        _configuration = configuration;
    }

    public async Task<ServiceResponse<int>> Register(User user, string password) {
        var response = new ServiceResponse<int>();
        CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
        if (await UserExists(user.Username)) {
            response.Success = false;
            response.Message = "User already exists";
            return response;
        }
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        _context.Add(user);
        await _context.SaveChangesAsync();
        response.Data = user.Id;
        return response;
    }

    public async Task<ServiceResponse<string>> Login(string username, string password) {
        var response = new ServiceResponse<string>();
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username.ToLower().Equals(username.ToLower()));
        if (user is null) {
            response.Success = false;
            response.Message = "User not found.";
        }
        else if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) {
            response.Success = false;
            response.Message = "Wrong password";
        }
        else {
            response.Data = CreateToken(user);
        }
        return response;
    }

    public async Task<bool> UserExists(string username) {
        return await _context.Users.AnyAsync(u => u.Username.ToLower().Equals(username.ToLower()));
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) {
        using (var hmac = new HMACSHA512()){
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt) {
        using (var hmac = new HMACSHA512(passwordSalt)) {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            var test = computedHash.SequenceEqual(passwordHash);
            return test;
        }
    }

    private string CreateToken(User user) {
        var claims = new List<Claim>() {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };
        var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;
        if (appSettingsToken is null)
            throw new Exception("AppSettings Token is null");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettingsToken));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var TokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = credentials
        };
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(TokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}