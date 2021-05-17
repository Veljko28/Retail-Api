using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace Retail_Api.Helpers
{
	public static class CheckRole
	{
		private static bool isJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
		{
			return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
				jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
		}

		public static ClaimsPrincipal getPrincipalFromToken(string token, IConfiguration configuration)
		{
			var tokenHandler = new JwtSecurityTokenHandler();

			JwtSettings JwtSettings = new JwtSettings();
			configuration.Bind(nameof(JwtSettings), JwtSettings);

			try
			{

				var tokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtSettings.Secret)),
					ValidateIssuer = false,
					ValidateAudience = false,
					RequireExpirationTime = false,
					ValidateLifetime = true
				};

				var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
				if (!isJwtWithValidSecurityAlgorithm(validatedToken))
				{
					return null;
				}

				return principal;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}

		}

		private static bool IsInRolePriv(string roleName, string token, IConfiguration configuration)
		{ 
			var validatedToken = getPrincipalFromToken(token, configuration);

			roleName = roleName.ToLower() == "admin" ? "adn" : roleName.ToLower() == "manager" ? "mng" : "csr";

			string inRole = validatedToken.Claims.SingleOrDefault(x => x.Type.ToLower() == roleName.ToLower())?.Value;

			if (string.IsNullOrEmpty(inRole))
			{
				return false;
			}

			if (inRole.ToLower() == "true")
			{
				return true;
			}

			return false;
		}
		

		public static bool IsInRole(StringValues authorization, string roleName, IConfiguration configuration)
		{
			if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
			{
				var token = headerValue.Parameter;
				if (!CheckRole.IsInRolePriv(roleName, token, configuration))
				{
					return false;
				}
			}
			else return false;

			return true;
		}
	}
}
