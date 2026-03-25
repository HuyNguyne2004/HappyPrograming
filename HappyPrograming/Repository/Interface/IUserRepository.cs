using HappyPrograming.Models;

namespace HappyPrograming.Repository.Interface
{
    public interface IUserRepository
    {
        User? GetUserByLogin(string username, string password);
    }
}
