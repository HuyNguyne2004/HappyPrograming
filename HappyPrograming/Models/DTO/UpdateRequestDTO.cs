namespace HappyPrograming.Models.DTO
{
    public class UpdateRequestDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateOnly Deadlinedate { get; set; }
        public int Deadlinehour { get; set; }
        public List<int> SkillIds { get; set; } = new List<int>();
    }
}
