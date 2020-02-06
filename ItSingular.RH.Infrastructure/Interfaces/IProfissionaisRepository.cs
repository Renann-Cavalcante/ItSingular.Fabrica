using System.Collections.Generic;
using System.Threading.Tasks;
using ItSingular.RH.Domain.Entities;

namespace ItSingular.RH.Infrastructure.Interfaces
{
	public interface IProfissionaisRepository
    {
		void Add<T>(T entity) where T : class;
		void Update<T>(T entity) where T : class;
		void Delete<T>(T entity) where T : class;
		Task<bool> SavelAll();

		Task<IEnumerable<Profissionais>> GetProfissionaisList();

		Task<Profissionais> GetProfissionaisById(int id);
	}
}
