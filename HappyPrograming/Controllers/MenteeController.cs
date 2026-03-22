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


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int menteeId = 3;

            var stats = await _menteeService.GetMenteeStatistics(menteeId);
            var requests = await _menteeRepository.GetRequestsByMenteeIdAsync(menteeId);

            ViewBag.Stats = stats;
            return View(requests);
        }


        [HttpGet]
        public async Task<IActionResult> Suggestions(int id)
        {
            int menteeId = 3;

            if (!await _menteeRepository.RequestOwnedByMenteeAsync(id, menteeId))
            {
                return NotFound();
            }

            var mentors = await _menteeService.GetSuggestedMentors(id);
            ViewBag.RequestId = id;
            return View(mentors);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Skills = await _context.Skills.Where(s => s.Status == "Active").ToListAsync();
            var model = new Request
            {
                Deadlinedate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                Deadlinehour = 1
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Request request, List<int>? skillIds)
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


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            int menteeId = 3;

            var request = await _menteeRepository.GetRequestByIdForMenteeAsync(id, menteeId);
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
            int menteeId = 3;

            var result = await _menteeService.UpdateMenteeRequest(dto, menteeId);

            if (result == "Success")
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = result;
            ViewBag.Skills = await _context.Skills.Where(s => s.Status == "Active").ToListAsync();
            return View(dto);
        }

        [HttpPost]
        [Consumes("application/json")]
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