using ApiProyects.Files;
using ApiProyects.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApiProyects.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDBContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDBContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public async Task Create(T entity)
        {
            await dbSet.AddAsync(entity);
            await Engrave();
        }

        public async Task Delete(T entity)
        {
            dbSet.Remove(entity);
            await Engrave();
        }

        public async Task Engrave()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null) query = query.Where(filter);
            return await query.ToListAsync();
        }

        public async Task<T> GetOne(Expression<Func<T, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;
            if (!tracked) query = query.AsNoTracking();
            if (filter != null) query = query.Where(filter);
            return await query.FirstOrDefaultAsync();
        }
    }
}
