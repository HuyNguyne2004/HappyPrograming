using System;
using System.Collections.Generic;

namespace HappyPrograming.Models;

public partial class Mentor
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Profession { get; set; } = null!;

    public string? ProfessionIntroduction { get; set; }

    public string? ServiceDescription { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();
}
