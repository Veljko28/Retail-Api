using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Retail_Api.Repositories.Interfaces
{
	public interface IGenericInterface<T, N> where T : class where N : class
	{
		Task<T> getByIdAsync(int id);
		Task<IEnumerable<T>> getAllAsync();
		Task<T> addAsync(N entity);
		Task<bool> deleteAsync(int id);
		Task<T> updateAsync(T entity);
	}
}
