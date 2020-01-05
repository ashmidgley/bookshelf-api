using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Api
{
    public class UserHelper : IUserHelper
    {
        private IConfiguration _config;

        public UserHelper(IConfiguration config)
        {
            _config = config;
        }

        public string BuildToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Issuer"], expires: DateTime.Now.AddMinutes(30), signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string HashPassword(string password, byte[] salt = null)
        {
            if(salt == null)
            {
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            }

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            var hash = pbkdf2.GetBytes(20);

            var hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }

        public bool PasswordsMatch(string password, string passwordHash, byte[] salt = null)
        {
            var hashBytes = Convert.FromBase64String(passwordHash);
            if(salt == null) 
            {
                salt = new byte[16];
                Array.Copy(hashBytes, 0, salt, 0, 16);
            }

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            var hash = pbkdf2.GetBytes(20);

            for(int i = 0; i < 20; i++)
            {
                if(hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}