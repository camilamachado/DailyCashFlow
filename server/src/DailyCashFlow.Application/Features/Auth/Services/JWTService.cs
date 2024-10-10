using DailyCashFlow.Domain.Features.Users;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DailyCashFlow.Application.Features.Auth.Services
{
	public interface IJWTService
	{
		/// <summary>
		/// Gera um token JWT para um usuário especificado.
		/// </summary>
		/// <param name="user">O usuário para o qual o token JWT deve ser gerado.</param>
		/// <returns>Um token JWT como uma string, que pode ser usado para autenticação em requisições futuras.</returns>
		public string GenerateJwtToken(User user);
	}

	public class JWTService : IJWTService
	{
		public JWTService() { }

		public string GenerateJwtToken(User user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes("super_secret_key_with_32_characters_or_more");

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
					new Claim(ClaimTypes.Email, user.Email),
                    // Adicione mais claims conforme necessário
                }),
				Expires = DateTime.UtcNow.AddHours(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}
	}
}
