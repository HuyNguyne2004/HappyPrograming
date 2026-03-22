using HappyPrograming.Models;
using HappyPrograming.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace HappyPrograming.Repository
{
    public class MenteeRepository
    {

        private readonly HappyprogrammingContext _context;

        public MenteeRepository(HappyprogrammingContext context)
        {
            _context = context;
        }
        public async Task<List<Request>> GetRequestsByMenteeIdAsync(int menteeId)
        {
            return await _context.Requests
                .Include(r => r.Skills) // Load kèm danh sách kỹ năng của request
                .Where(r => r.CreatorId == menteeId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Mentor>> GetMentorSuggestionsAsync(int menteeId)
        {
            var menteeSkillIds = await _context.Requests
                .Where(r => r.CreatorId == menteeId && r.Status == "Open")
                .SelectMany(r => r.Skills)
                .Select(s => s.Id)
                .Distinct()
                .ToListAsync();

            return await _context.Mentors
                .Include(m => m.User)
                .Include(m => m.Skills)
                .Where(m => m.Skills.Any(s => menteeSkillIds.Contains(s.Id)))
                .ToListAsync();
        }

        public async Task SaveRequestAsync(Request request, List<int> skillIds)
        {
            var selectedSkills = await _context.Skills
                .Where(s => skillIds.Contains(s.Id))
                .ToListAsync();

            request.Skills = selectedSkills;

            _context.Requests.Add(request);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Request>> GetRequestsWithMentorsByMenteeIdAsync(int menteeId)
        {
            return await _context.Requests
                .Include(r => r.Mentors)
                .Where(r => r.CreatorId == menteeId)
                .ToListAsync();
        }

        public async Task<bool> UpdateRequestAsync(UpdateRequestDTO dto)
        {
            var existingRequest = await _context.Requests
                .Include(r => r.Skills)
                .FirstOrDefaultAsync(r => r.Id == dto.Id);

            if (existingRequest == null) return false;

            existingRequest.Title = dto.Title;
            existingRequest.Content = dto.Content;
            existingRequest.Deadlinedate = dto.Deadlinedate;
            existingRequest.Deadlinehour = dto.Deadlinehour;

            existingRequest.Skills.Clear();
            var newSkills = await _context.Skills
                .Where(s => dto.SkillIds.Contains(s.Id))
                .ToListAsync();

            foreach (var skill in newSkills)
            {
                existingRequest.Skills.Add(skill);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SaveFeedbackAsync(Feedback feedback)
        {
            try
            {
                _context.Feedbacks.Add(feedback);
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Mentor>> GetSuggestedMentorsByRequestAsync(int requestId)
        {
            var requestSkillIds = await _context.Requests
                .Where(r => r.Id == requestId)
                .SelectMany(r => r.Skills)
                .Select(s => s.Id)
                .ToListAsync();

            return await _context.Mentors
                .Include(m => m.User)
                .Include(m => m.Feedbacks)
                .Include(m => m.Requests)
                .Where(m => m.Skills.Any(s => requestSkillIds.Contains(s.Id)))
                .ToListAsync();
        }
    }
}
