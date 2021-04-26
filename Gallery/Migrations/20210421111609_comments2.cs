using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gallery.Migrations
{
    public partial class comments2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_Commentguid",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Files_StoredFileId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_Commentguid",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_StoredFileId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Commentguid",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "StoredFileId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "child",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "datetime",
                table: "Comments",
                newName: "Datetime");

            migrationBuilder.AddColumn<Guid>(
                name: "ChildOfId",
                table: "Comments",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "HasComments",
                table: "Comments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsChild",
                table: "Comments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "parentguid",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ChildOfId",
                table: "Comments",
                column: "ChildOfId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_parentguid",
                table: "Comments",
                column: "parentguid");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Files_ChildOfId",
                table: "Comments",
                column: "ChildOfId",
                principalTable: "Files",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_parentguid",
                table: "Comments",
                column: "parentguid",
                principalTable: "Comments",
                principalColumn: "guid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Files_ChildOfId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_parentguid",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ChildOfId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_parentguid",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ChildOfId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "HasComments",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "IsChild",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "parentguid",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "Datetime",
                table: "Comments",
                newName: "datetime");

            migrationBuilder.AddColumn<Guid>(
                name: "Commentguid",
                table: "Comments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StoredFileId",
                table: "Comments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "child",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_Commentguid",
                table: "Comments",
                column: "Commentguid");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_StoredFileId",
                table: "Comments",
                column: "StoredFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_Commentguid",
                table: "Comments",
                column: "Commentguid",
                principalTable: "Comments",
                principalColumn: "guid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Files_StoredFileId",
                table: "Comments",
                column: "StoredFileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
