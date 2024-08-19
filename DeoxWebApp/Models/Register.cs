using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeoxWebApp.Models;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using static SessionBuffer;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace DeoxWebApp.Models
{
    public class Register
    {
        public static SqlConnection connection = new SqlConnection("Data Source=800166NB\\MSSQLSERVER01;Initial Catalog=UserInfo;Integrated Security=True;");

        public bool IsValidUsername(string username)
        {
            return username.Length >= 5;
        }

        public bool IsValidPassword(string password)
        {
            if (password.Length < 5)
                return false;
            if (!password.Any(char.IsUpper))
                return false;
            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                return false;
            return true;
        }

        public bool UsernameEmailControl(string username, string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                throw new ArgumentException("Geçerli bir e-posta adresi giriniz.");
            }

            try
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username OR EMAIL = @Email";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Email", email);

                    int count = (int)command.ExecuteScalar();
                    return count == 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Database error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }


        private string _encrypt_idenity = "mahmut";
        private string _encrypt_changepassword = "base64_encoded_secret_key_fd_mg_sdf";

        public string EncryptMD5Function(string input, string key)
        {
            DataLayer dataLayer = new DataLayer();
            Register register = new Register();
            byte[] data = UTF8Encoding.UTF8.GetBytes(input);
            switch (key)
            {
                case ("password"):

                    using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                    {
                        byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(_encrypt_idenity));
                        using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider()
                        {
                            Key = keys,
                            Mode = CipherMode.ECB,
                            Padding = PaddingMode.PKCS7
                        })
                        {
                            ICryptoTransform transform = tripDes.CreateEncryptor();
                            byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                            return Convert.ToBase64String(results, 0, results.Length);
                        }
                    }               
            }
            return "";
        }



        public string GenerateToken(int userId, int requestId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_encrypt_changepassword);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("userId", userId.ToString()),
                new Claim("requestId", requestId.ToString()),
                new Claim("privateKey", _encrypt_changepassword)
            }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public (int userId, int requestId) ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_encrypt_changepassword);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out SecurityToken validatedToken);

                var userId = Convert.ToInt32(principal.FindFirst("userId")?.Value);
                var requestId = Convert.ToInt32(principal.FindFirst("requestId")?.Value);

                return (userId, requestId);
            }
            catch
            {
                // Token geçersiz
                return (0, 0);
            }
        }

       


        public bool RegisterUser(string username, string email, string hashedPassword)
        {
            try
            {
                connection.Open();
                string query = "INSERT INTO Users (Username, EMAIL, Hash_Pass) VALUES (@Username, @Email, @Hash_Pass)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Hash_Pass", hashedPassword);

                    int result = command.ExecuteNonQuery();
                    return result > 0; // Kayıt başarılı ise true döner
                }
            }
            catch (Exception ex)
            {
                // Hata işlemleri
                throw new Exception("Database error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
