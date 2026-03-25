using HappyPrograming.Models;
using Microsoft.EntityFrameworkCore;

namespace HappyPrograming.Repository
{
    public class MentorRepository
    {
        private readonly HappyprogrammingContext _context;

        public MentorRepository(HappyprogrammingContext context)
        {
            _context = context;
        }


        public Mentor? GetMentorProfile(int id)
        {
            return _context.Mentors
                .Include(m => m.User)
                .Include(m => m.Skills)
                .FirstOrDefault(m => m.Id == id);
        }

        public bool ValidateUserPassword(int userId, string oldPassword)
        {
            var user = _context.Users.Find(userId);
            return user != null && user.Password == oldPassword;
        }


        public bool UpdatePassword(int userId, string newPassword)
        {
            var user = _context.Users.Find(userId);
            if (user == null) return false;

            user.Password = newPassword;
            _context.SaveChanges();
            return true;
        }
    }
}
