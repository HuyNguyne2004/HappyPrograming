using System;
using System.Collections.Generic;

namespace HappyPrograming.Models;

public partial class Feedback
{
    public int Id { get; set; }

    public int RequestId { get; set; }

    public int MenteeId { get; set; }

    public int MentorId { get; set; }

    public double RatingStar { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User Mentee { get; set; } = null!;

    public virtual Mentor Mentor { get; set; } = null!;

    public virtual Request Request { get; set; } = null!;
}
