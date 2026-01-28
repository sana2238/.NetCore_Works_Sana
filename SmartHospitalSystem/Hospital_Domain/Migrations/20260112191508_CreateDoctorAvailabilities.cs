using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_Domain.Migrations
{
    /// <inheritdoc />
    public partial class CreateDoctorAvailabilities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
    name: "DoctorAvailabilities",
    columns: table => new
    {
        Id = table.Column<int>(nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
        DoctorId = table.Column<Guid>(nullable: false),
        DayOfWeek = table.Column<int>(nullable: false),
        FromTime = table.Column<TimeSpan>(nullable: false),
        ToTime = table.Column<TimeSpan>(nullable: false),
        IsAvailable = table.Column<bool>(nullable: false)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_DoctorAvailabilities", x => x.Id);
        table.ForeignKey(
            name: "FK_DoctorAvailabilities_Doctors_DoctorId",
            column: x => x.DoctorId,
            principalTable: "Doctors",
            principalColumn: "DoctorId",
            onDelete: ReferentialAction.Cascade);
    });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
    name: "DoctorAvailabilities");

        }
    }
}
