using HappyPrograming.Models;
using HappyPrograming.Models.DTO;
using HappyPrograming.Repository;

namespace HappyPrograming.Service
{
    public class MenteeService
    {
        private readonly MenteeRepository _menteeRepository;
        public MenteeService(MenteeRepository menteeRepository)
        {
            _menteeRepository = menteeRepository;
        }

        public async Task<string> CreateNewRequest(Request request, List<int> skillIds)
        {
            if (string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Content))
            {
                return "Title and Content cannot be empty.";
            }

            if (skillIds == null || !skillIds.Any())
            {
                return "Please select at least one skill.";
            }

            if (request.Deadlinedate < DateOnly.FromDateTime(DateTime.Now))
            {
                return "Deadline date must be in the future.";
            }

            try
            {
                request.Status = "Open";
                request.CreatedAt = DateTime.Now;

                await _menteeRepository.SaveRequestAsync(request, skillIds);

                return "Success";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }


        public async Task<StatisticDTO> GetMenteeStatistics(int menteeId)
        {
            var requests = await _menteeRepository.GetRequestsWithMentorsByMenteeIdAsync(menteeId);

            var dto = new StatisticDTO
            {
                RequestTitles = requests.Select(r => r.Title).ToList(),

                TotalRequests = requests.Count,


                TotalHours = requests.Sum(r => r.Deadlinehour),

                TotalMentors = requests
                    .SelectMany(r => r.Mentors)
                    .Select(m => m.Id)
                    .Distinct()
                    .Count()
            };

            return dto;
        }

        public async Task<string> UpdateMenteeRequest(UpdateRequestDTO dto)
        {

            if (string.IsNullOrEmpty(dto.Title) || string.IsNullOrEmpty(dto.Content))
            {
                return "Title and Content are required.";
            }

            if (dto.SkillIds == null || !dto.SkillIds.Any())
            {
                return "Please select at least one skill.";
            }

            if (dto.Deadlinedate < DateOnly.FromDateTime(DateTime.Now))
            {
                return "Deadline cannot be in the past.";
            }

            try
            {

                bool isUpdated = await _menteeRepository.UpdateRequestAsync(dto);

                if (isUpdated)
                {
                    return "Success";
                }
                else
                {
                    return "Request not found.";
                }
            }
            catch (Exception ex)
            {
                return $"Update failed: {ex.Message}";
            }
        }
        public async Task<string> SaveFeedback(RatingDTO dto, int menteeId)
        {
            if (dto.Star < 1 || dto.Star > 5)
            {
                return "Invalid rating star. Please rate from 1 to 5.";
            }


            var feedback = new Feedback
            {
                RequestId = dto.RequestId,
                MenteeId = menteeId,
                MentorId = dto.MentorId,
                RatingStar = dto.Star,
                Comment = dto.Comment
            };


            bool isSaved = await _menteeRepository.SaveFeedbackAsync(feedback);

            if (isSaved)
            {
                return "Success";
            }

            return "Failed to save feedback.";
        }


        public async Task<List<MentorSuggestionDTO>> GetSuggestedMentors(int requestId)
        {
            var mentors = await _menteeRepository.GetSuggestedMentorsByRequestAsync(requestId);


            var suggestionList = mentors.Select(m => new MentorSuggestionDTO
            {
                MentorId = m.Id,
                FullName = $"{m.User.FirstName} {m.User.LastName}",
                AccountName = m.User.Username,


                AverageStar = m.Feedbacks.Any()
                    ? (float)m.Feedbacks.Average(f => f.RatingStar)
                    : 0,

                CurrentRequests = m.Requests.Count(r => r.Status != "Closed" && r.Status != "Canceled")
            }).ToList();

            return suggestionList;
        }

    }
}
