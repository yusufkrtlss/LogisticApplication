using LogisticApi.Dtos.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace LogisticApi.Helpers
{
	public class TokenHandler
	{
		public IConfiguration Configuration { get; set; }
		public TokenHandler(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public TokenDto CreateAccessToken()
		{
			TokenDto tokenInstance = new TokenDto();

			SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:SecurityKey"]));

			SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			tokenInstance.Expiration = DateTime.Now.AddHours(10);
			JwtSecurityToken securityToken = new JwtSecurityToken(
				issuer: Configuration["Token:Issuer"],
				audience: Configuration["Token:Audience"],
				expires: tokenInstance.Expiration,
				notBefore: DateTime.Now,
				signingCredentials: signingCredentials
			);

			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

			tokenInstance.AccessToken = tokenHandler.WriteToken(securityToken);

			tokenInstance.RefreshToken = CreateRefreshToken();
			return tokenInstance;
		}

		public string CreateRefreshToken()
		{
			byte[] number = new byte[32];
			using (RandomNumberGenerator random = RandomNumberGenerator.Create())
			{
				random.GetBytes(number);
				return Convert.ToBase64String(number);
			}
		}
	}
}
