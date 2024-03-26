using ApiProyects.Models;

namespace ApiProyects.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> Update(User entity);
    }
}
