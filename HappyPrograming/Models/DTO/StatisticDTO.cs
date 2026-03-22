namespace HappyPrograming.Models.DTO
{
    public class StatisticDTO
    {
        public List<string> RequestTitles { get; set; } = new List<string>();
        public int TotalRequests { get; set; }
        public int TotalHours { get; set; }
        public int TotalMentors { get; set; }
    }
}
