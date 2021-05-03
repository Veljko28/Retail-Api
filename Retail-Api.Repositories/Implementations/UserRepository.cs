using Microsoft.Extensions.Configuration;
using Retail_Api.Models;
using Retail_Api.Models.Requests;
using Retail_Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace Retail_Api.Repositories.Implementations
{
	public class UserRepository : IUserRepository
	{
		private readonly IConfiguration _configuration;

		public UserRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public Task<bool> deleteUserAsync(string token)
		{
			throw new NotImplementedException();
		}

		public Task<User> getByTokenAsync(string token)
		{
			throw new NotImplementedException();
		}

		public Task<User> updateUserAsync(User entity)
		{
			throw new NotImplementedException();
		}
	}
}
