using Fullstack.API.Data;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace Fullstack.API.Utils
{
    public class PasswordHasher
    {
        private readonly IConfiguration _configuration;

        public PasswordHasher(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string Hash(string password)
        {
            string salt = "!$@#%$";
            string passwordHashed = null;
            using (var md5 = MD5.Create())
            {
                passwordHashed = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(password + salt))).Replace("-", "");
            }
            // Format hash with extra information
            return passwordHashed;
        }
    }
}
