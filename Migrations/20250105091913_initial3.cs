using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppServer.Migrations
{
    /// <inheritdoc />
    public partial class initial3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$SeDxPnwY4fP4k0N9zvU9K.SveDXKROhseTxbYqK2mmWta668F3fXG");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$SeDxPnwY4fP4k0N9zvU9K.SveDXKROhseTxbYqK2mmWta668F3fXG");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$SeDxPnwY4fP4k0N9zvU9K.SveDXKROhseTxbYqK2mmWta668F3fXG");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
