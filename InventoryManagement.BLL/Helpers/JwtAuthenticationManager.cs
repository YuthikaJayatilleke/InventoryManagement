using DevExpress.Xpo;
using InventoryManagement.BE.User;
using InventoryManagement.BLL.Interfaces.BLL.Helpers;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InventoryManagement.BLL.Helpers
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private string _key;
        public JwtAuthenticationManager(string key)
        {
            _key = key;
        }
        public string Authenticate(User user)
        {
            var tokenHandeler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_key);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.Username+"#"+user.Password)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandeler.CreateToken(tokenDescription);
            return tokenHandeler.WriteToken(token);
        }
    }

}