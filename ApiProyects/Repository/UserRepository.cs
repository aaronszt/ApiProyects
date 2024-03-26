using ApiProyects.Files;
using ApiProyects.Models;
using ApiProyects.Repository.IRepository;

namespace ApiProyects.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDBContext _db;

        public UserRepository(ApplicationDBContext db): base(db)
        {
            _db = db;
        }
        public async Task<User> Update(User entity)
        {
            entity.UpdateDate = DateTime.Now;
            _db.Users.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
