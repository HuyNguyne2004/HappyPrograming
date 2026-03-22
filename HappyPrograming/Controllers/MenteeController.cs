using HappyPrograming.Models;
using HappyPrograming.Models.DTO;
using HappyPrograming.Repository;
using HappyPrograming.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HappyPrograming.Controllers
{
    public class MenteeController : Controller
    {
        private readonly MenteeService _menteeService;
        private readonly MenteeRepository _menteeRepository;
        private readonly HappyprogrammingContext _context;

        public MenteeController(MenteeService menteeService, MenteeRepository menteeRepository, HappyprogrammingContext context)
        {
            _menteeService = menteeService;
            _menteeRepository = menteeRepository;
            _context = context;
        }


        public async Task<IActionResult> Index()
        {

            int menteeId = 3;

            var stats = await _menteeService.GetMenteeStatistics(menteeId);
            var requests = await _menteeRepository.GetRequestsByMenteeIdAsync(menteeId);

            ViewBag.Stats = stats;
            return View(requests);
        }


        public async Task<IActionResult> Suggestions(int id)
        {
            var mentors = await _menteeService.GetSuggestedMentors(id);
            ViewBag.RequestId = id;
            return View(mentors);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Skills = await _context.Skills.Where(s => s.Status == "Active").ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Request request, List<int> skillIds)
        {
            request.CreatorId = 3;

            var result = await _menteeService.CreateNewRequest(request, skillIds);

            if (result == "Success")
            {
                return RedirectToAction(nameof(Index));
            }


            ViewBag.Error = result;
            ViewBag.Skills = await _context.Skills.Where(s => s.Status == "Active").ToListAsync();
            return View(request);
        }


        public async Task<IActionResult> Update(int id)
        {
            var request = await _context.Requests
                .Include(r => r.Skills)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null) return NotFound();


            var dto = new UpdateRequestDTO
            {
                Id = request.Id,
                Title = request.Title,
                Content = request.Content,
                Deadlinedate = request.Deadlinedate,
                Deadlinehour = request.Deadlinehour,
                SkillIds = request.Skills.Select(s => s.Id).ToList()
            };

            ViewBag.Skills = await _context.Skills.Where(s => s.Status == "Active").ToListAsync();
            return View(dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateRequestDTO dto)
        {
            var result = await _menteeService.UpdateMenteeRequest(dto);

            if (result == "Success")
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = result;
            ViewBag.Skills = await _context.Skills.Where(s => s.Status == "Active").ToListAsync();
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> RateMentor([FromBody] RatingDTO dto)
        {
            int menteeId = 3;

            var result = await _menteeService.SaveFeedback(dto, menteeId);

            if (result == "Success")
            {
                return Json(new { success = true, message = "Cảm ơn bạn đã đánh giá!" });
            }

            return Json(new { success = false, message = result });
        }
    }
}