using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vrossi.ScaffoldCore.Infrastructure.Persistence.Migrations
{
    public partial class ImproveAccountProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                schema: "dbo",
                table: "AccountProfile",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                schema: "dbo",
                table: "AccountProfile",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }
    }
}
