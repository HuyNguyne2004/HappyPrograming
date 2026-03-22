using Microsoft.EntityFrameworkCore;

namespace HappyPrograming.Models;

public partial class HappyprogrammingContext : DbContext
{
    public HappyprogrammingContext()
    {
    }

    public HappyprogrammingContext(DbContextOptions<HappyprogrammingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Feature> Features { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Mentor> Mentors { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("Default");
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Feature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_feature");

            entity.ToTable("feature");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .HasColumnName("url");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_feedback");

            entity.ToTable("feedback");

            entity.HasIndex(e => new { e.RequestId, e.MenteeId }, "uq_feedback_mentee").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.MenteeId).HasColumnName("mentee_id");
            entity.Property(e => e.MentorId).HasColumnName("mentor_id");
            entity.Property(e => e.RatingStar).HasColumnName("rating_star");
            entity.Property(e => e.RequestId).HasColumnName("request_id");

            entity.HasOne(d => d.Mentee).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.MenteeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_feedback_mentee");

            entity.HasOne(d => d.Mentor).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.MentorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_feedback_mentor");

            entity.HasOne(d => d.Request).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_feedback_request");
        });

        modelBuilder.Entity<Mentor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_mentor");

            entity.ToTable("mentor");

            entity.HasIndex(e => e.UserId, "UQ__mentor__B9BE370EC682AD34").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Profession)
                .HasMaxLength(255)
                .HasColumnName("profession");
            entity.Property(e => e.ProfessionIntroduction).HasColumnName("profession_introduction");
            entity.Property(e => e.ServiceDescription).HasColumnName("service_description");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.Mentor)
                .HasForeignKey<Mentor>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_mentor_user");

            entity.HasMany(d => d.Requests).WithMany(p => p.Mentors)
                .UsingEntity<Dictionary<string, object>>(
                    "MentorRequest",
                    r => r.HasOne<Request>().WithMany()
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_mentorrequest_request"),
                    l => l.HasOne<Mentor>().WithMany()
                        .HasForeignKey("MentorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_mentorrequest_mentor"),
                    j =>
                    {
                        j.HasKey("MentorId", "RequestId").HasName("pk_mentorrequest");
                        j.ToTable("mentor_request");
                        j.IndexerProperty<int>("MentorId").HasColumnName("mentor_id");
                        j.IndexerProperty<int>("RequestId").HasColumnName("request_id");
                    });

            entity.HasMany(d => d.Skills).WithMany(p => p.Mentors)
                .UsingEntity<Dictionary<string, object>>(
                    "MentorSkill",
                    r => r.HasOne<Skill>().WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_mentorskill_skill"),
                    l => l.HasOne<Mentor>().WithMany()
                        .HasForeignKey("MentorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_mentorskill_mentor"),
                    j =>
                    {
                        j.HasKey("MentorId", "SkillId").HasName("pk_mentorskill");
                        j.ToTable("mentor_skill");
                        j.IndexerProperty<int>("MentorId").HasColumnName("mentor_id");
                        j.IndexerProperty<int>("SkillId").HasColumnName("skill_id");
                    });
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_request");

            entity.ToTable("request");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatorId).HasColumnName("creator_id");
            entity.Property(e => e.Deadlinedate).HasColumnName("deadlinedate");
            entity.Property(e => e.Deadlinehour).HasColumnName("deadlinehour");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Open")
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Creator).WithMany(p => p.Requests)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_request_mentee");

            entity.HasMany(d => d.Skills).WithMany(p => p.Requests)
                .UsingEntity<Dictionary<string, object>>(
                    "RequestSkill",
                    r => r.HasOne<Skill>().WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_requestskill_skill"),
                    l => l.HasOne<Request>().WithMany()
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_requestskill_request"),
                    j =>
                    {
                        j.HasKey("RequestId", "SkillId").HasName("pk_requestskill");
                        j.ToTable("request_skill");
                        j.IndexerProperty<int>("RequestId").HasColumnName("request_id");
                        j.IndexerProperty<int>("SkillId").HasColumnName("skill_id");
                    });
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_role");

            entity.ToTable("role");

            entity.HasIndex(e => e.Name, "UQ__role__72E12F1B718CD346").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.HasMany(d => d.Features).WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "RoleFeature",
                    r => r.HasOne<Feature>().WithMany()
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_rolefeature_feature"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_rolefeature_role"),
                    j =>
                    {
                        j.HasKey("RoleId", "FeatureId").HasName("pk_rolefeature");
                        j.ToTable("role_feature");
                        j.IndexerProperty<int>("RoleId").HasColumnName("role_id");
                        j.IndexerProperty<int>("FeatureId").HasColumnName("feature_id");
                    });
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_skill");

            entity.ToTable("skill");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValue("Active")
                .HasColumnName("status");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_user");

            entity.ToTable("user");

            entity.HasIndex(e => e.EmailAddress, "UQ__user__20C6DFF58201DFF5").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "UQ__user__A1936A6B0E0D7E03").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__user__F3DBC572F13F87AC").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Avatar)
                .HasMaxLength(255)
                .HasColumnName("avatar");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(255)
                .HasColumnName("email_address");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(30)
                .HasColumnName("phone_number");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValue("Active")
                .HasColumnName("status");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_role");
        });

        modelBuilder.Entity<Role>().HasData(
              new Role { Id = 1, Name = "Admin" },
              new Role { Id = 2, Name = "Mentor" },
              new Role { Id = 3, Name = "Mentee" }
          );

        // 2. Seed Skills
        modelBuilder.Entity<Skill>().HasData(
            new Skill { Id = 1, Name = "C# Programming", Description = "Basic to Advanced C#", Status = "Active" },
            new Skill { Id = 2, Name = "Java", Description = "Java Core", Status = "Active" },
            new Skill { Id = 3, Name = "SQL Server", Description = "Database Management", Status = "Active" }
        );

        // 3. Seed Users (PHẢI ĐỦ TỪ ID 1 ĐẾN 7 ĐỂ PHỤC VỤ MENTOR/REQUEST)
        modelBuilder.Entity<User>().HasData(
      new User { Id = 1, Username = "admin01", Password = "password123", FirstName = "Admin", LastName = "System", Gender = "Other", Dob = new DateTime(1990, 1, 1), PhoneNumber = "0123456789", EmailAddress = "admin@happy.com", RoleId = 1, Status = "Active", CreatedAt = DateTime.Now },
      new User { Id = 2, Username = "mentor_khoa", Password = "password123", FirstName = "Khoa", LastName = "Nguyen", Gender = "Male", Dob = new DateTime(1995, 5, 20), PhoneNumber = "0987654321", EmailAddress = "khoa@mentor.com", RoleId = 2, Status = "Active", CreatedAt = DateTime.Now },
      new User { Id = 3, Username = "mentee_an", Password = "password123", FirstName = "An", LastName = "Tran", Gender = "Female", Dob = new DateTime(2002, 10, 15), PhoneNumber = "0909090909", EmailAddress = "an@mentee.com", RoleId = 3, Status = "Active", CreatedAt = DateTime.Now },
      new User { Id = 4, Username = "mentor_lan", Password = "password123", FirstName = "Lan", LastName = "Hoang", Gender = "Female", Dob = new DateTime(1992, 3, 10), PhoneNumber = "0911223344", EmailAddress = "lan.hoang@mentor.com", RoleId = 2, Status = "Active", CreatedAt = DateTime.Now },
      new User { Id = 5, Username = "mentor_minh", Password = "password123", FirstName = "Minh", LastName = "Vu", Gender = "Male", Dob = new DateTime(1988, 8, 25), PhoneNumber = "0922334455", EmailAddress = "minh.vu@mentor.com", RoleId = 2, Status = "Active", CreatedAt = DateTime.Now },
      new User { Id = 6, Username = "mentee_long", Password = "password123", FirstName = "Long", LastName = "Phi", Gender = "Male", Dob = new DateTime(2003, 1, 12), PhoneNumber = "0933445566", EmailAddress = "long.phi@mentee.com", RoleId = 3, Status = "Active", CreatedAt = DateTime.Now },
      new User { Id = 7, Username = "mentee_vy", Password = "password123", FirstName = "Vy", LastName = "Le", Gender = "Female", Dob = new DateTime(2004, 11, 30), PhoneNumber = "0944556677", EmailAddress = "vy.le@mentee.com", RoleId = 3, Status = "Active", CreatedAt = DateTime.Now }
  );

        // 4. Seed Mentor (UserId PHẢI TỒN TẠI TRONG USER)
        modelBuilder.Entity<Mentor>().HasData(
            new Mentor { Id = 1, UserId = 2, Profession = "Senior .NET Developer", ProfessionIntroduction = "5 years of experience", ServiceDescription = "Teaching C# and EF Core" },
            new Mentor { Id = 2, UserId = 4, Profession = "Frontend Specialist", ProfessionIntroduction = "ReactJS Expert", ServiceDescription = "Teaching React and UI/UX" },
            new Mentor { Id = 3, UserId = 5, Profession = "Data Scientist", ProfessionIntroduction = "Python Specialist", ServiceDescription = "Teaching SQL and ML" }
        );

        // 5. Seed Request (CreatorId PHẢI TỒN TẠI TRONG USER)
        modelBuilder.Entity<Request>().HasData(
            new Request { Id = 1, CreatorId = 3, Title = "Lỗi kết nối DbContext", Deadlinedate = new DateOnly(2026, 4, 1), Deadlinehour = 2, Content = "Fix bug scaffold.", Status = "Open" },
            new Request { Id = 2, CreatorId = 6, Title = "Học ReactJS từ đầu", Deadlinedate = new DateOnly(2026, 4, 15), Deadlinehour = 10, Content = "Lộ trình học React.", Status = "Processing" },
            new Request { Id = 3, CreatorId = 7, Title = "Tối ưu câu lệnh SQL", Deadlinedate = new DateOnly(2026, 3, 25), Deadlinehour = 5, Content = "Review câu lệnh JOIN.", Status = "Open" },
            new Request { Id = 4, CreatorId = 3, Title = "Review đồ án", Deadlinedate = new DateOnly(2026, 5, 10), Deadlinehour = 20, Content = "Góp ý kiến trúc.", Status = "Open" },
            new Request { Id = 5, CreatorId = 6, Title = "Docker căn bản", Deadlinedate = new DateOnly(2026, 4, 20), Deadlinehour = 3, Content = "Build container cho .NET.", Status = "Closed" }
        );

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
