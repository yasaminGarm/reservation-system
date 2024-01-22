using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeanScene.Data.Migrations
{
    /// <inheritdoc />
    public partial class origionchange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ReservationOrigions",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Online");

            migrationBuilder.UpdateData(
                table: "ReservationOrigions",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Via Phone");

            migrationBuilder.UpdateData(
                table: "ReservationOrigions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Staff Create a Reservation", "Via Email" });

            migrationBuilder.UpdateData(
                table: "ReservationOrigions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Staff Create a Reservation", "Walked in" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ReservationOrigions",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Customer");

            migrationBuilder.UpdateData(
                table: "ReservationOrigions",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Staff");

            migrationBuilder.UpdateData(
                table: "ReservationOrigions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Register as a Member", "Customer-Register as a Member" });

            migrationBuilder.UpdateData(
                table: "ReservationOrigions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Admin Creates a Reservation", "Admin" });
        }
    }
}
