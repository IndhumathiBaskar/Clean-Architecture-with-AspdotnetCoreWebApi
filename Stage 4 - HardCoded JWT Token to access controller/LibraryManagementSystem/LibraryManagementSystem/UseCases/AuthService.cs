using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Security.Claims;

namespace LibraryManagementSystem.UseCases
{
    public class AuthService
    {
        private readonly string _secretKey;

        public AuthService(string secretKey)
        {
            _secretKey = secretKey;
        }


        /// <summary>
        /// Generates a JSON Web Token (JWT) for a given username, including the username as a claim and setting an expiration time.
        /// The token is signed using the application's secret key and the HMAC-SHA256 algorithm.
        /// </summary>
        /// <param name="username">The username to be included in the token's claims.</param>
        /// <returns>A string representing the generated JWT.</returns>
        /// <remarks>
        /// This method performs the following steps:
        /// 1. Initializes a JwtSecurityTokenHandler to handle JWT creation and serialization.
        /// 2. Converts the application's secret key into a byte array using UTF-8 encoding.
        /// 3. Creates a SecurityTokenDescriptor to define the token's properties:
        ///    - Sets the token's subject to a ClaimsIdentity containing a Name claim with the provided username.
        ///    - Sets the token's expiration time to one hour from the current UTC time.
        ///    - Configures the signing credentials using a symmetric key derived from the secret key and the HMAC-SHA256 algorithm.
        /// 4. Creates the JWT using the token descriptor.
        /// 5. Serializes the created JWT into a string and returns it.
        /// </remarks>
        public string GenerateToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}
