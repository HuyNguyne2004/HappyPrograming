namespace HappyPrograming.Models.DTO
{
    public class MentorSuggestionDTO
    {
        public int MentorId { get; set; }
        public string FullName { get; set; }
        public string AccountName { get; set; }
        public float AverageStar { get; set; }
        public int CurrentRequests { get; set; }
    }
}
