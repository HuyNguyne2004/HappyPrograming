using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HappyPrograming.Migrations
{
    /// <inheritdoc />
    public partial class addData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "feature",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_feature", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "skill",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, defaultValue: "Active")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_skill", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role_feature",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "int", nullable: false),
                    feature_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rolefeature", x => new { x.role_id, x.feature_id });
                    table.ForeignKey(
                        name: "fk_rolefeature_feature",
                        column: x => x.feature_id,
                        principalTable: "feature",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_rolefeature_role",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    avatar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    first_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    dob = table.Column<DateOnly>(type: "date", nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    email_address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, defaultValue: "Active"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_role",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "mentor",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    profession = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    profession_introduction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    service_description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mentor", x => x.id);
                    table.ForeignKey(
                        name: "fk_mentor_user",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "request",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    creator_id = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    deadlinedate = table.Column<DateOnly>(type: "date", nullable: false),
                    deadlinehour = table.Column<int>(type: "int", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Open"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_request", x => x.id);
                    table.ForeignKey(
                        name: "fk_request_mentee",
                        column: x => x.creator_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "mentor_skill",
                columns: table => new
                {
                    mentor_id = table.Column<int>(type: "int", nullable: false),
                    skill_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mentorskill", x => new { x.mentor_id, x.skill_id });
                    table.ForeignKey(
                        name: "fk_mentorskill_mentor",
                        column: x => x.mentor_id,
                        principalTable: "mentor",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_mentorskill_skill",
                        column: x => x.skill_id,
                        principalTable: "skill",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "feedback",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    request_id = table.Column<int>(type: "int", nullable: false),
                    mentee_id = table.Column<int>(type: "int", nullable: false),
                    mentor_id = table.Column<int>(type: "int", nullable: false),
                    rating_star = table.Column<double>(type: "float", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_feedback", x => x.id);
                    table.ForeignKey(
                        name: "fk_feedback_mentee",
                        column: x => x.mentee_id,
                        principalTable: "user",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_feedback_mentor",
                        column: x => x.mentor_id,
                        principalTable: "mentor",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_feedback_request",
                        column: x => x.request_id,
                        principalTable: "request",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "mentor_request",
                columns: table => new
                {
                    mentor_id = table.Column<int>(type: "int", nullable: false),
                    request_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mentorrequest", x => new { x.mentor_id, x.request_id });
                    table.ForeignKey(
                        name: "fk_mentorrequest_mentor",
                        column: x => x.mentor_id,
                        principalTable: "mentor",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_mentorrequest_request",
                        column: x => x.request_id,
                        principalTable: "request",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "request_skill",
                columns: table => new
                {
                    request_id = table.Column<int>(type: "int", nullable: false),
                    skill_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_requestskill", x => new { x.request_id, x.skill_id });
                    table.ForeignKey(
                        name: "fk_requestskill_request",
                        column: x => x.request_id,
                        principalTable: "request",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_requestskill_skill",
                        column: x => x.skill_id,
                        principalTable: "skill",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                table: "role",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Mentor" },
                    { 3, "Mentee" }
                });

            migrationBuilder.InsertData(
                table: "skill",
                columns: new[] { "id", "description", "name", "status" },
                values: new object[,]
                {
                    { 1, "Basic to Advanced C#", "C# Programming", "Active" },
                    { 2, "Java Core", "Java", "Active" },
                    { 3, "Database Management", "SQL Server", "Active" }
                });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "id", "address", "avatar", "created_at", "dob", "email_address", "first_name", "gender", "last_name", "password", "phone_number", "role_id", "status", "username" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(2026, 3, 22, 11, 55, 23, 21, DateTimeKind.Local).AddTicks(286), new DateOnly(1990, 1, 1), "admin@happy.com", "Admin", "Other", "System", "password123", "0123456789", 1, "Active", "admin01" },
                    { 2, null, null, new DateTime(2026, 3, 22, 11, 55, 23, 21, DateTimeKind.Local).AddTicks(303), new DateOnly(1995, 5, 20), "khoa@mentor.com", "Khoa", "Male", "Nguyen", "password123", "0987654321", 2, "Active", "mentor_khoa" },
                    { 3, null, null, new DateTime(2026, 3, 22, 11, 55, 23, 21, DateTimeKind.Local).AddTicks(308), new DateOnly(2002, 10, 15), "an@mentee.com", "An", "Female", "Tran", "password123", "0909090909", 3, "Active", "mentee_an" },
                    { 4, null, null, new DateTime(2026, 3, 22, 11, 55, 23, 21, DateTimeKind.Local).AddTicks(314), new DateOnly(1992, 3, 10), "lan.hoang@mentor.com", "Lan", "Female", "Hoang", "password123", "0911223344", 2, "Active", "mentor_lan" },
                    { 5, null, null, new DateTime(2026, 3, 22, 11, 55, 23, 21, DateTimeKind.Local).AddTicks(318), new DateOnly(1988, 8, 25), "minh.vu@mentor.com", "Minh", "Male", "Vu", "password123", "0922334455", 2, "Active", "mentor_minh" },
                    { 6, null, null, new DateTime(2026, 3, 22, 11, 55, 23, 21, DateTimeKind.Local).AddTicks(323), new DateOnly(2003, 1, 12), "long.phi@mentee.com", "Long", "Male", "Phi", "password123", "0933445566", 3, "Active", "mentee_long" },
                    { 7, null, null, new DateTime(2026, 3, 22, 11, 55, 23, 21, DateTimeKind.Local).AddTicks(328), new DateOnly(2004, 11, 30), "vy.le@mentee.com", "Vy", "Female", "Le", "password123", "0944556677", 3, "Active", "mentee_vy" }
                });

            migrationBuilder.InsertData(
                table: "mentor",
                columns: new[] { "id", "profession", "profession_introduction", "service_description", "user_id" },
                values: new object[,]
                {
                    { 1, "Senior .NET Developer", "5 years of experience", "Teaching C# and EF Core", 2 },
                    { 2, "Frontend Specialist", "ReactJS Expert", "Teaching React and UI/UX", 4 },
                    { 3, "Data Scientist", "Python Specialist", "Teaching SQL and ML", 5 }
                });

            migrationBuilder.InsertData(
                table: "request",
                columns: new[] { "id", "content", "creator_id", "deadlinedate", "deadlinehour", "status", "title" },
                values: new object[,]
                {
                    { 1, "Fix bug scaffold.", 3, new DateOnly(2026, 4, 1), 2, "Open", "Lỗi kết nối DbContext" },
                    { 2, "Lộ trình học React.", 6, new DateOnly(2026, 4, 15), 10, "Processing", "Học ReactJS từ đầu" },
                    { 3, "Review câu lệnh JOIN.", 7, new DateOnly(2026, 3, 25), 5, "Open", "Tối ưu câu lệnh SQL" },
                    { 4, "Góp ý kiến trúc.", 3, new DateOnly(2026, 5, 10), 20, "Open", "Review đồ án" },
                    { 5, "Build container cho .NET.", 6, new DateOnly(2026, 4, 20), 3, "Closed", "Docker căn bản" }
                });

            migrationBuilder.InsertData(
                table: "feedback",
                columns: new[] { "id", "comment", "created_at", "mentee_id", "mentor_id", "rating_star", "request_id" },
                values: new object[,]
                {
                    { 1, "Rất nhiệt tình!", new DateTime(2026, 3, 22, 11, 55, 23, 21, DateTimeKind.Local).AddTicks(681), 6, 1, 5.0, 5 },
                    { 2, "Giải thích dễ hiểu.", new DateTime(2026, 3, 22, 11, 55, 23, 21, DateTimeKind.Local).AddTicks(685), 6, 1, 4.0, 2 },
                    { 3, "Cảm ơn anh Khoa đã giúp!", new DateTime(2026, 3, 22, 11, 55, 23, 21, DateTimeKind.Local).AddTicks(688), 3, 1, 5.0, 1 }
                });

            migrationBuilder.InsertData(
                table: "mentor_request",
                columns: new[] { "mentor_id", "request_id" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 1, 3 }
                });

            migrationBuilder.InsertData(
                table: "mentor_skill",
                columns: new[] { "mentor_id", "skill_id" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 3 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "request_skill",
                columns: new[] { "request_id", "skill_id" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 3 },
                    { 3, 3 },
                    { 4, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_feedback_mentee_id",
                table: "feedback",
                column: "mentee_id");

            migrationBuilder.CreateIndex(
                name: "IX_feedback_mentor_id",
                table: "feedback",
                column: "mentor_id");

            migrationBuilder.CreateIndex(
                name: "uq_feedback_mentee",
                table: "feedback",
                columns: new[] { "request_id", "mentee_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__mentor__B9BE370EC682AD34",
                table: "mentor",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mentor_request_request_id",
                table: "mentor_request",
                column: "request_id");

            migrationBuilder.CreateIndex(
                name: "IX_mentor_skill_skill_id",
                table: "mentor_skill",
                column: "skill_id");

            migrationBuilder.CreateIndex(
                name: "IX_request_creator_id",
                table: "request",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "IX_request_skill_skill_id",
                table: "request_skill",
                column: "skill_id");

            migrationBuilder.CreateIndex(
                name: "UQ__role__72E12F1B718CD346",
                table: "role",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_feature_feature_id",
                table: "role_feature",
                column: "feature_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_id",
                table: "user",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "UQ__user__20C6DFF58201DFF5",
                table: "user",
                column: "email_address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__user__A1936A6B0E0D7E03",
                table: "user",
                column: "phone_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__user__F3DBC572F13F87AC",
                table: "user",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "feedback");

            migrationBuilder.DropTable(
                name: "mentor_request");

            migrationBuilder.DropTable(
                name: "mentor_skill");

            migrationBuilder.DropTable(
                name: "request_skill");

            migrationBuilder.DropTable(
                name: "role_feature");

            migrationBuilder.DropTable(
                name: "mentor");

            migrationBuilder.DropTable(
                name: "request");

            migrationBuilder.DropTable(
                name: "skill");

            migrationBuilder.DropTable(
                name: "feature");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "role");
        }
    }
}
