using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HappyPrograming.Models;

[Table("user")]
public partial class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("username")]
    public string Username { get; set; } = null!;

    [Column("password")]
    public string Password { get; set; } = null!;

    [Column("avatar")]
    public string? Avatar { get; set; }

    [Column("first_name")]
    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    public string LastName { get; set; } = null!;

    [Column("gender")]
    public string Gender { get; set; } = null!;

    // ⚠️ FIX DateOnly
    [Column("dob")]
    public DateTime Dob { get; set; }   // đổi sang DateTime

    [Column("phone_number")]
    public string PhoneNumber { get; set; } = null!;

    [Column("email_address")]
    public string EmailAddress { get; set; } = null!;

    [Column("address")]
    public string? Address { get; set; }

    [Column("role_id")]
    public int RoleId { get; set; }

    [Column("status")]
    public string? Status { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual Mentor? Mentor { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual Role Role { get; set; } = null!;
}