using Retail_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Retail_Api.Services
{
	public interface IRefreshTokenDb
	{
		Task<RefreshToken> findTokenAsync(string Token);

		Task<bool> addTokenAsync(RefreshToken refreshToken);

		Task<bool> updateTokenAsync(RefreshToken refreshToken);

	}
}
