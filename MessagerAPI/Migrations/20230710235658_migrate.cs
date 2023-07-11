using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class migrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Conversations",
                newName: "ConversationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ConversationId",
                table: "Conversations",
                newName: "Id");
        }
    }
}
