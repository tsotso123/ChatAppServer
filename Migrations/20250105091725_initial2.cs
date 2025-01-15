using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppServer.Migrations
{
    /// <inheritdoc />
    public partial class initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Receipts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Received = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Receipts_Messeges_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messeges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$v5nToWNyJYKwgW8CEPu1e.9w.kiiOiTDdgzpQMErcIFCrXOXdFPFu");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$v5nToWNyJYKwgW8CEPu1e.9w.kiiOiTDdgzpQMErcIFCrXOXdFPFu");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$v5nToWNyJYKwgW8CEPu1e.9w.kiiOiTDdgzpQMErcIFCrXOXdFPFu");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_MessageId",
                table: "Receipts",
                column: "MessageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Receipts");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$3inofM7lB9xHByWoMOLY2eiBKAKnXlyMb6pvgPdKu3Z.78HbIvlWa");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$3inofM7lB9xHByWoMOLY2eiBKAKnXlyMb6pvgPdKu3Z.78HbIvlWa");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$3inofM7lB9xHByWoMOLY2eiBKAKnXlyMb6pvgPdKu3Z.78HbIvlWa");
        }
    }
}
