using HappyPrograming.Models;
using HappyPrograming.Repository.Interface;

namespace HappyPrograming.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly HappyprogrammingContext _context;

        public UserRepository(HappyprogrammingContext context)
        {
            _context = context;
        }

        public User? GetUserByLogin(string username, string password)
        {
            return _context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
