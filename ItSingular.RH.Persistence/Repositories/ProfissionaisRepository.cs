using ItSingular.RH.Domain.Entities;
using ItSingular.RH.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItSingular.RH.Persistence.Repositories
{
	public class ProfissionaisRepository : IProfissionaisRepository
    {
        private readonly ItSingularDbContext _context;

        public ProfissionaisRepository(ItSingularDbContext itSingularDbContext)
        {
            _context = itSingularDbContext;
        }

		public void Add<T>(T entity) where T : class
		{
			_context.Add(entity);
		}

		public void Update<T>(T entity) where T : class
		{
			_context.Entry(entity).State = EntityState.Modified;
		}

		public void Delete<T>(T entity) where T : class
		{
			_context.Remove(entity);
		}

		public async Task<bool> SavelAll()
		{
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<IEnumerable<Profissionais>> GetProfissionaisList()
		{
			var entities = await _context.Profissionais.ToListAsync();
			return entities;
		}

		public async Task<Profissionais> GetProfissionaisById(int id)
		{
			var entity = await _context.Profissionais.FirstOrDefaultAsync(p => p.Id == id);
			return entity;
		}

	}
}
