using System;
using System.Collections.Generic;

namespace HappyPrograming.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<Feature> Features { get; set; } = new List<Feature>();
}
