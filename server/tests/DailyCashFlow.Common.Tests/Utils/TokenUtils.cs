using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DailyCashFlow.Common.Tests.Utils
{
	public static class TokenUtils
	{
		public static string GerarTokenJWTFalso()
		{
			// Configura chave secreta e credenciais de assinatura
			var chaveSecreta = "super_secret_key_with_32_characters_or_more"; // Deve ser a mesma chave usada na configuração da API
			var chaveSimetrica = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveSecreta));
			var credenciais = new SigningCredentials(chaveSimetrica, SecurityAlgorithms.HmacSha256);

			// Defini as claims (informações) do token
			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, "user_id"),
				new Claim(JwtRegisteredClaimNames.Email, "usuario@exemplo.com"),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			// Gera o token JWT
			var token = new JwtSecurityToken(
				issuer: "DailyCashFlow",
				audience: "DailyCashFlow",
				claims: claims,
				expires: DateTime.Now.AddMinutes(30),
				signingCredentials: credenciais
			);

			// Retorna o token JWT como string
			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
