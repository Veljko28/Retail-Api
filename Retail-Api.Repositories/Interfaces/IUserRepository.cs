﻿using Retail_Api.Models;
using Retail_Api.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Retail_Api.Repositories.Interfaces
{
	public interface IUserRepository
	{
		Task<bool> deleteUserAsync(string token);
		Task<User> getByTokenAsync(string token);
		Task<User> updateUserAsync(User entity);
		
	}
}