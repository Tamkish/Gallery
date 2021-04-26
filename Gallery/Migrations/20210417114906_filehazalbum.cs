using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gallery.Migrations
{
    public partial class filehazalbum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Albums_AlbumOwnerId_AlbumName",
                table: "Files");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Files",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Files",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    guid = table.Column<Guid>(nullable: false),
                    AuthorId = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    datetime = table.Column<DateTime>(nullable: false),
                    child = table.Column<bool>(nullable: false),
                    Commentguid = table.Column<Guid>(nullable: true),
                    StoredFileId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.guid);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_Commentguid",
                        column: x => x.Commentguid,
                        principalTable: "Comments",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Files_StoredFileId",
                        column: x => x.StoredFileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_Commentguid",
                table: "Comments",
                column: "Commentguid");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_StoredFileId",
                table: "Comments",
                column: "StoredFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Albums_AlbumOwnerId_AlbumName",
                table: "Files",
                columns: new[] { "AlbumOwnerId", "AlbumName" },
                principalTable: "Albums",
                principalColumns: new[] { "OwnerId", "Name" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Albums_AlbumOwnerId_AlbumName",
                table: "Files");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Files");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Albums_AlbumOwnerId_AlbumName",
                table: "Files",
                columns: new[] { "AlbumOwnerId", "AlbumName" },
                principalTable: "Albums",
                principalColumns: new[] { "OwnerId", "Name" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
