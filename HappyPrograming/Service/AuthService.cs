using HappyPrograming.Models;
using HappyPrograming.Repository.Interface;

namespace HappyPrograming.Service
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User? Login(string username, string password)
        {
            // Thực hiện logic validate cơ bản trước khi xuống Repo
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return null;

            return _userRepository.GetUserByLogin(username, password);
        }
    }
}
