using HappyPrograming.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HappyProgramming.Controllers
{
    [Authorize(Roles = "Admin")] // Bảo mật - chỉ Admin mới vào được
    public class AdminController : Controller
    {
        private readonly HappyprogrammingContext _context;

        public AdminController(HappyprogrammingContext context)
        {
            _context = context;
        }

        // =====================
        // TRANG CHÍNH ADMIN DASHBOARD (Trang tổng quan)
        // =====================
        public IActionResult Index()
        {
            // Khi truy cập /Admin hoặc /Admin/ → tự động chuyển sang Dashboard
            return RedirectToAction("Dashboard");
        }

        public IActionResult Dashboard()
        {
            // Truyền thống kê nhanh để hiển thị trên dashboard
            ViewBag.TotalMentors = _context.Mentors.Count();
            ViewBag.TotalRequests = _context.Requests.Count();
            ViewBag.TotalMentees = _context.Users.Count(u => u.RoleId == 3);
            ViewBag.TotalSkills = _context.Skills.Count();

            // Bạn có thể thêm các thống kê khác nếu muốn
            // Ví dụ: ViewBag.TotalOpenRequests = _context.Requests.Count(r => r.Status == "Open");

            return View();
        }

        // =====================
        // 1. VIEW ALL MENTORS (#20)
        // =====================
        public async Task<IActionResult> Mentors(string keyword, int page = 1)
        {
            int pageSize = 10;

            var query = _context.Mentors
                .Include(m => m.User)
                .Include(m => m.Requests)
                .Include(m => m.Feedbacks)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim().ToLower();
                query = query.Where(m =>
                    (m.User.FirstName + " " + m.User.LastName).ToLower().Contains(keyword) ||
                    m.User.Username.ToLower().Contains(keyword) ||
                    m.Profession.ToLower().Contains(keyword));
            }

            var totalItems = await query.CountAsync();

            var mentors = await query
                .OrderBy(m => m.User.LastName)
                .ThenBy(m => m.User.FirstName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Keyword = keyword;
            ViewBag.Page = page;
            ViewBag.Total = totalItems;
            ViewBag.PageSize = pageSize;

            return View(mentors);
        }

        // =====================
        // TOGGLE MENTOR STATUS
        // =====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleMentorStatus(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null && user.RoleId == 2) // Chỉ toggle cho Mentor
            {
                user.Status = user.Status == "Active" ? "Inactive" : "Active";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Mentors");
        }

        // =====================
        // 2. VIEW ALL REQUESTS (#21)
        // =====================
        public async Task<IActionResult> Requests(string keyword, string status, DateTime? start, DateTime? end, int page = 1)
        {
            int pageSize = 10;

            var query = _context.Requests
                .Include(r => r.Creator)
                .Include(r => r.Skills)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim().ToLower();
                query = query.Where(r =>
                    r.Title.ToLower().Contains(keyword) ||
                    r.Creator.Username.ToLower().Contains(keyword));
            }

            if (!string.IsNullOrEmpty(status))
                query = query.Where(r => r.Status == status);

            if (start.HasValue)
                query = query.Where(r => r.CreatedAt >= start.Value);

            if (end.HasValue)
                query = query.Where(r => r.CreatedAt <= end.Value.Date.AddDays(1).AddTicks(-1));

            var totalItems = await query.CountAsync();

            var requests = await query
                .OrderByDescending(r => r.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Keyword = keyword;
            ViewBag.Status = status;
            ViewBag.Start = start?.ToString("yyyy-MM-dd");
            ViewBag.End = end?.ToString("yyyy-MM-dd");
            ViewBag.Page = page;
            ViewBag.Total = totalItems;
            ViewBag.PageSize = pageSize;

            return View(requests);
        }

        public async Task<IActionResult> RequestDetail(int id)
        {
            var request = await _context.Requests
                .Include(r => r.Skills)
                .Include(r => r.Creator)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
                return NotFound();

            return View(request);
        }

        // =====================
        // 3. STATISTIC MENTEE (#22)
        // =====================
        public async Task<IActionResult> MenteeStatistics()
        {
            var data = await _context.Users
                .Where(u => u.RoleId == 3)
                .Select(u => new
                {
                    Fullname = u.FirstName + " " + u.LastName,
                    u.Username,
                    TotalRequests = u.Requests.Count(),
                    TotalHours = u.Requests.Sum(r => r.Deadlinehour),
                    TotalSkills = u.Requests.SelectMany(r => r.Skills).Distinct().Count()
                })
                .OrderBy(x => x.Fullname)
                .ToListAsync();

            return View(data);
        }

        // =====================
        // 4. SKILL MANAGEMENT (#23)
        // =====================
        public async Task<IActionResult> Skills()
        {
            var skills = await _context.Skills
                .OrderBy(s => s.Name)
                .ToListAsync();

            return View(skills);
        }

        // =====================
        // TOGGLE SKILL STATUS
        // =====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleSkill(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill != null)
            {
                skill.Status = skill.Status == "Active" ? "Inactive" : "Active";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Skills");
        }

        // =====================
        // 5. CREATE SKILL (#24)
        // =====================
        [HttpGet]
        public IActionResult CreateSkill()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSkill(Skill model)
        {
            if (ModelState.IsValid)
            {
                model.Status = "Active";
                _context.Skills.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Thêm kỹ năng thành công!";
                return RedirectToAction("Skills");
            }
            return View(model);
        }

        // =====================
        // 6. UPDATE SKILL (#25)
        // =====================
        [HttpGet]
        public async Task<IActionResult> UpdateSkill(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null) return NotFound();
            return View(skill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSkill(Skill model)
        {
            if (ModelState.IsValid)
            {
                _context.Skills.Update(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật kỹ năng thành công!";
                return RedirectToAction("Skills");
            }
            return View(model);
        }
    }
}