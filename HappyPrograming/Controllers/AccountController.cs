using HappyPrograming.Models;
using HappyPrograming.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HappyPrograming.Controllers
{
    public class AccountController : Controller
    {
        private readonly HappyprogrammingContext _context;
        private readonly OTPService _otpService;
        private readonly SendMailService _sendMail;

        public AccountController(HappyprogrammingContext context, OTPService otpService, SendMailService sendMail)
        {
            _context = context;
            _otpService = otpService;
            _sendMail = sendMail;
        }

        // GET: Hiển thị form đăng ký
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(User userModel)
        {
            // Kiểm tra dữ liệu đầu vào (Khớp bước 3.1.1 validateInput trong sơ đồ)
            if (string.IsNullOrEmpty(userModel.Gender))
            {
                userModel.Gender = "Other"; // Gán giá trị mặc định nếu null
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == userModel.EmailAddress);

            if (existingUser == null)
            {
                userModel.Status = "Pending";
                userModel.CreatedAt = DateTime.Now;

                // Nếu DB yêu cầu RoleId mà form không gửi, hãy gán cứng ở đây
                if (userModel.RoleId == 0) userModel.RoleId = 2;

                _context.Users.Add(userModel);
                await _context.SaveChangesAsync(); // Sẽ không còn lỗi NULL nữa
            }

            // Luồng OTP tiếp theo...
            var otp = _otpService.GenerateOTP(userModel.EmailAddress);
            await _sendMail.SendEmailAsync(userModel.EmailAddress, "OTP", $"Mã: {otp}");

            return RedirectToAction("VerifyOTP", new { email = userModel.EmailAddress });
        }

        [HttpGet]
        public IActionResult VerifyOTP(string email) => View(model: email);

        // 8.1: VerifyOTP
        [HttpPost]
        public async Task<IActionResult> VerifyOTP(string email, string otp)
        {
            if (_otpService.ValidateOTP(email, otp)) // [OTP valid]
            {
                // 8.1.1: UpdateAsync (Xác nhận thành công)
                var user = await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == email);
                if (user != null)
                {
                    user.Status = "Active";
                    await _context.SaveChangesAsync();
                }

                // 9: Success message
                return Content("<h1>Đăng ký thành công! Hãy quay lại trang Login.</h1>");
            }

            // 10: OTP invalid
            ViewBag.Error = "Mã OTP không đúng hoặc đã hết hạn!";
            return View(model: email);
        }
    }
}