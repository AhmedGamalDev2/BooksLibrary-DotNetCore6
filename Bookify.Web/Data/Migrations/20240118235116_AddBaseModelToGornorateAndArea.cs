using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
    public partial class AddBaseModelToGornorateAndArea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Governorates_GovernmentId",
                table: "Areas");

            migrationBuilder.RenameColumn(
                name: "GovernmentId",
                table: "Areas",
                newName: "GovernorateId");

            migrationBuilder.RenameIndex(
                name: "IX_Areas_GovernmentId",
                table: "Areas",
                newName: "IX_Areas_GovernorateId");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Governorates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Governorates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Governorates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedById",
                table: "Governorates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedOn",
                table: "Governorates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Areas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Areas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Areas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedById",
                table: "Areas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedOn",
                table: "Areas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Governorates_GovernorateId",
                table: "Areas",
                column: "GovernorateId",
                principalTable: "Governorates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Governorates_GovernorateId",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Governorates");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Governorates");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Governorates");

            migrationBuilder.DropColumn(
                name: "LastUpdatedById",
                table: "Governorates");

            migrationBuilder.DropColumn(
                name: "LastUpdatedOn",
                table: "Governorates");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "LastUpdatedById",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "LastUpdatedOn",
                table: "Areas");

            migrationBuilder.RenameColumn(
                name: "GovernorateId",
                table: "Areas",
                newName: "GovernmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Areas_GovernorateId",
                table: "Areas",
                newName: "IX_Areas_GovernmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Governorates_GovernmentId",
                table: "Areas",
                column: "GovernmentId",
                principalTable: "Governorates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
