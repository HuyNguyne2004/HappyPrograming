namespace HappyPrograming.Models.DTO
{
    public class MentorSuggestionDTO
    {
        public int MentorId { get; set; }
        public string FullName { get; set; } = null!;
        public string AccountName { get; set; } = null!;
        public float AverageStar { get; set; }
        public int CurrentRequests { get; set; }
    }
}
