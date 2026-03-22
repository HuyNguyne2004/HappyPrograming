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

        public async Task<string> CreateNewRequest(Request request, List<int>? skillIds)
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


                TotalHours = requests.Sum(r => (double)r.Deadlinehour),

                TotalMentors = requests
                    .SelectMany(r => r.Mentors)
                    .Select(m => m.Id)
                    .Distinct()
                    .Count()
            };

            return dto;
        }

        public async Task<string> UpdateMenteeRequest(UpdateRequestDTO dto, int menteeId)
        {
            if (string.IsNullOrWhiteSpace(dto.Title) || string.IsNullOrWhiteSpace(dto.Content))
            {
                return "Vui lòng nhập đầy đủ tiêu đề và nội dung.";
            }

            if (dto.SkillIds == null || !dto.SkillIds.Any())
            {
                return "Vui lòng chọn ít nhất một kỹ năng.";
            }

            if (dto.Deadlinedate < DateOnly.FromDateTime(DateTime.Now))
            {
                return "Ngày hết hạn không được trong quá khứ.";
            }

            try
            {
                bool isUpdated = await _menteeRepository.UpdateRequestAsync(dto, menteeId);

                if (isUpdated)
                {
                    return "Success";
                }

                return "Không tìm thấy yêu cầu hoặc bạn không có quyền chỉnh sửa.";
            }
            catch (Exception ex)
            {
                return $"Cập nhật thất bại: {ex.Message}";
            }
        }
        public async Task<string> SaveFeedback(RatingDTO dto, int menteeId)
        {
            if (dto.Star < 1 || dto.Star > 5)
            {
                return "Số sao phải từ 1 đến 5.";
            }

            if (!await _menteeRepository.RequestOwnedByMenteeAsync(dto.RequestId, menteeId))
            {
                return "Không tìm thấy yêu cầu hoặc bạn không có quyền đánh giá.";
            }

            if (await _menteeRepository.MenteeHasFeedbackForRequestAsync(dto.RequestId, menteeId))
            {
                return "Bạn đã gửi đánh giá cho yêu cầu này.";
            }

            var feedback = new Feedback
            {
                RequestId = dto.RequestId,
                MenteeId = menteeId,
                MentorId = dto.MentorId,
                RatingStar = dto.Star,
                Comment = string.IsNullOrWhiteSpace(dto.Comment) ? null : dto.Comment.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            bool isSaved = await _menteeRepository.SaveFeedbackAsync(feedback);

            if (isSaved)
            {
                return "Success";
            }

            return "Không thể lưu đánh giá. Vui lòng thử lại.";
        }


        public async Task<List<MentorSuggestionDTO>> GetSuggestedMentors(int requestId)
        {
            var mentors = await _menteeRepository.GetSuggestedMentorsByRequestAsync(requestId);

            return mentors
                .Select(m => new MentorSuggestionDTO
                {
                    MentorId = m.Id,
                    FullName = $"{m.User.FirstName} {m.User.LastName}",
                    AccountName = m.User.Username,
                    AverageStar = m.Feedbacks.Count > 0
                        ? (float)m.Feedbacks.Average(f => f.RatingStar)
                        : 0f,
                    CurrentRequests = m.Requests.Count(r => r.Status != "Closed" && r.Status != "Canceled")
                })
                .OrderByDescending(d => d.AverageStar)
                .ThenBy(d => d.FullName)
                .ToList();
        }

    }
}
