using System;
using System.Collections.Generic;

namespace HappyPrograming.Models;

public partial class Skill
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Mentor> Mentors { get; set; } = new List<Mentor>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
