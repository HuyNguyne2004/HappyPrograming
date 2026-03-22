namespace HappyPrograming.Models.DTO
{
    public class RatingDTO
    {
        public int RequestId { get; set; }
        public int MentorId { get; set; }
        public int Star { get; set; }
        public string? Comment { get; set; }
    }
}
