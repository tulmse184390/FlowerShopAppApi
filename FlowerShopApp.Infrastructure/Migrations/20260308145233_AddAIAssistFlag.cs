using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowerShopApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAIAssistFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_ai_assisted",
                table: "chat_rooms",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_ai_assisted",
                table: "chat_rooms");
        }
    }
}
