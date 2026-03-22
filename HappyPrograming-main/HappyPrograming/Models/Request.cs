using System;
using System.Collections.Generic;

namespace HappyPrograming.Models;

public partial class Request
{
    public int Id { get; set; }

    public int CreatorId { get; set; }

    public string Title { get; set; } = null!;

    public DateOnly Deadlinedate { get; set; }

    public int Deadlinehour { get; set; }

    public string Content { get; set; } = null!;

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User Creator { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Mentor> Mentors { get; set; } = new List<Mentor>();

    public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();
}
