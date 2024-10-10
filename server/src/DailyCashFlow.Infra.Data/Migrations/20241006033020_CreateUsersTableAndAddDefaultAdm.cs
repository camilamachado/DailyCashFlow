using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailyCashFlow.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateUsersTableAndAddDefaultAdm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    IsAdministrator = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

			migrationBuilder.InsertData(
	            table: "Users",
	            columns: new[] { "Id", "Name", "Email", "Password", "IsAdministrator" },
	            values: new object[] {
				            1,
                            "Admin",
                            "admin@admin.com",
							"$2a$11$cksU8pOFP2/pe9P.MtYRfOT4QwgWcWjVdk57Aa1EwAIxLslR3n48i",
                            true
	            }
            );
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
