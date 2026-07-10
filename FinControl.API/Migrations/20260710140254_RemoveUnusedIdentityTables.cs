using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FinControl.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnusedIdentityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_papel_claims");

            migrationBuilder.DropTable(
                name: "tb_usuario_logins");

            migrationBuilder.DropTable(
                name: "tb_usuario_papeis");

            migrationBuilder.DropTable(
                name: "tb_papeis");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_papeis",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_papeis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_usuario_logins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_usuario_logins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_tb_usuario_logins_tb_usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "tb_usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_papel_claims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_papel_claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_papel_claims_tb_papeis_RoleId",
                        column: x => x.RoleId,
                        principalTable: "tb_papeis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_usuario_papeis",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_usuario_papeis", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_tb_usuario_papeis_tb_papeis_RoleId",
                        column: x => x.RoleId,
                        principalTable: "tb_papeis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_usuario_papeis_tb_usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "tb_usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "tb_papeis",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_papel_claims_RoleId",
                table: "tb_papel_claims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_usuario_logins_UserId",
                table: "tb_usuario_logins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_usuario_papeis_RoleId",
                table: "tb_usuario_papeis",
                column: "RoleId");
        }
    }
}
