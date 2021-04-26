using Microsoft.EntityFrameworkCore.Migrations;

namespace Gallery.Migrations
{
    public partial class albumz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AlbumName",
                table: "Files",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AlbumOwnerId",
                table: "Files",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    OwnerId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Public = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => new { x.OwnerId, x.Name });
                    table.ForeignKey(
                        name: "FK_Albums_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_AlbumOwnerId_AlbumName",
                table: "Files",
                columns: new[] { "AlbumOwnerId", "AlbumName" });

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Albums_AlbumOwnerId_AlbumName",
                table: "Files",
                columns: new[] { "AlbumOwnerId", "AlbumName" },
                principalTable: "Albums",
                principalColumns: new[] { "OwnerId", "Name" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Albums_AlbumOwnerId_AlbumName",
                table: "Files");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropIndex(
                name: "IX_Files_AlbumOwnerId_AlbumName",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "AlbumName",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "AlbumOwnerId",
                table: "Files");
        }
    }
}
