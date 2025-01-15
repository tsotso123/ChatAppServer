using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAppServer.Migrations
{
    /// <inheritdoc />
    public partial class addedForthAcc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$QFSF5h8ZS0FEr2.w2gLNxuAN/9fwoyYXSShJfi4DOsMusjBTKKKvW");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$11$QFSF5h8ZS0FEr2.w2gLNxuAN/9fwoyYXSShJfi4DOsMusjBTKKKvW");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$11$QFSF5h8ZS0FEr2.w2gLNxuAN/9fwoyYXSShJfi4DOsMusjBTKKKvW");

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "CreatedAt", "FirstName", "LastLogin", "LastName", "Password", "Username" },
                values: new object[] { 4, null, "adam4", null, "baruch", "$2a$11$QFSF5h8ZS0FEr2.w2gLNxuAN/9fwoyYXSShJfi4DOsMusjBTKKKvW", "tso" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 4);

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
    }
}
