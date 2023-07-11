using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class changeMessageModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SenderId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverEmail",
                table: "Messages",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SenderEmail",
                table: "Messages",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverEmail",
                table: "Messages",
                column: "ReceiverEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderEmail",
                table: "Messages",
                column: "SenderEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_ReceiverEmail",
                table: "Messages",
                column: "ReceiverEmail",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SenderEmail",
                table: "Messages",
                column: "SenderEmail",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_ReceiverEmail",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SenderEmail",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ReceiverEmail",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SenderEmail",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ReceiverEmail",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SenderEmail",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "SenderId",
                table: "Messages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
