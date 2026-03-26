using HappyPrograming.Repository;
using Microsoft.AspNetCore.Mvc;

namespace HappyPrograming.Controllers
{
    public class MentorController : Controller
    {
        private readonly MentorRepository _mentorRepo;

        public MentorController(MentorRepository mentorRepo)
        {
            _mentorRepo = mentorRepo;
        }

        // FLOW A: View Mentor Profile
        [HttpGet]
        public IActionResult Profile(int id)
        {
            // 1.1: requestProfile(mentorId)
            var mentor = _mentorRepo.GetMentorProfile(id);
            if (mentor == null) return NotFound();

            return View(mentor); // 1.1.2: return profile data
        }

        // NEW FLOW: Change Password
        [HttpPost]
        public IActionResult ChangePassword(int userId, string oldPass, string newPass, string confirm)
        {
            // Kiểm tra khớp mật khẩu mới trước
            if (newPass != confirm)
            {
                ViewBag.Error = "Xác nhận mật khẩu không khớp!";
                return View("Profile");
            }

            // 3: validateUserAndOldPassword
            bool isValid = _mentorRepo.ValidateUserPassword(2, oldPass);

            if (isValid) // [isValid = true]
            {
                // 6a: updatePassword
                _mentorRepo.UpdatePassword(2, newPass);
                ViewBag.Success = "Đổi mật khẩu thành công!"; // 8a: return success
            }
            else // [isValid = false]
            {
                ViewBag.Error = "Mật khẩu cũ không chính xác!"; // 6b: return failure
            }

            return View("Profile"); // 9: display result
        }
    }
}
