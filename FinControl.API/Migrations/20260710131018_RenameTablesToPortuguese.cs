using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinControl.API.Migrations
{
    /// <inheritdoc />
    public partial class RenameTablesToPortuguese : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Goals",
                table: "Goals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BudgetLimits",
                table: "BudgetLimits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "tb_transacoes");

            migrationBuilder.RenameTable(
                name: "Goals",
                newName: "tb_metas");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "tb_categorias");

            migrationBuilder.RenameTable(
                name: "BudgetLimits",
                newName: "tb_limites_orcamento");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "tb_usuario_tokens");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "tb_usuarios");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "tb_usuario_papeis");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "tb_usuario_logins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "tb_usuario_claims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "tb_papeis");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "tb_papel_claims");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_UserId_Nome",
                table: "tb_categorias",
                newName: "IX_tb_categorias_UserId_Nome");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetLimits_UserId_Categoria",
                table: "tb_limites_orcamento",
                newName: "IX_tb_limites_orcamento_UserId_Categoria");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "tb_usuario_papeis",
                newName: "IX_tb_usuario_papeis_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "tb_usuario_logins",
                newName: "IX_tb_usuario_logins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "tb_usuario_claims",
                newName: "IX_tb_usuario_claims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "tb_papel_claims",
                newName: "IX_tb_papel_claims_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_transacoes",
                table: "tb_transacoes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_metas",
                table: "tb_metas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_categorias",
                table: "tb_categorias",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_limites_orcamento",
                table: "tb_limites_orcamento",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_usuario_tokens",
                table: "tb_usuario_tokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_usuarios",
                table: "tb_usuarios",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_usuario_papeis",
                table: "tb_usuario_papeis",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_usuario_logins",
                table: "tb_usuario_logins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_usuario_claims",
                table: "tb_usuario_claims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_papeis",
                table: "tb_papeis",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_papel_claims",
                table: "tb_papel_claims",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_papel_claims_tb_papeis_RoleId",
                table: "tb_papel_claims",
                column: "RoleId",
                principalTable: "tb_papeis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_usuario_claims_tb_usuarios_UserId",
                table: "tb_usuario_claims",
                column: "UserId",
                principalTable: "tb_usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_usuario_logins_tb_usuarios_UserId",
                table: "tb_usuario_logins",
                column: "UserId",
                principalTable: "tb_usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_usuario_papeis_tb_papeis_RoleId",
                table: "tb_usuario_papeis",
                column: "RoleId",
                principalTable: "tb_papeis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_usuario_papeis_tb_usuarios_UserId",
                table: "tb_usuario_papeis",
                column: "UserId",
                principalTable: "tb_usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_usuario_tokens_tb_usuarios_UserId",
                table: "tb_usuario_tokens",
                column: "UserId",
                principalTable: "tb_usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_papel_claims_tb_papeis_RoleId",
                table: "tb_papel_claims");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_usuario_claims_tb_usuarios_UserId",
                table: "tb_usuario_claims");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_usuario_logins_tb_usuarios_UserId",
                table: "tb_usuario_logins");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_usuario_papeis_tb_papeis_RoleId",
                table: "tb_usuario_papeis");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_usuario_papeis_tb_usuarios_UserId",
                table: "tb_usuario_papeis");

            migrationBuilder.DropForeignKey(
                name: "FK_tb_usuario_tokens_tb_usuarios_UserId",
                table: "tb_usuario_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_usuarios",
                table: "tb_usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_usuario_tokens",
                table: "tb_usuario_tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_usuario_papeis",
                table: "tb_usuario_papeis");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_usuario_logins",
                table: "tb_usuario_logins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_usuario_claims",
                table: "tb_usuario_claims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_transacoes",
                table: "tb_transacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_papel_claims",
                table: "tb_papel_claims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_papeis",
                table: "tb_papeis");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_metas",
                table: "tb_metas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_limites_orcamento",
                table: "tb_limites_orcamento");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_categorias",
                table: "tb_categorias");

            migrationBuilder.RenameTable(
                name: "tb_usuarios",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "tb_usuario_tokens",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "tb_usuario_papeis",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "tb_usuario_logins",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "tb_usuario_claims",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "tb_transacoes",
                newName: "Transactions");

            migrationBuilder.RenameTable(
                name: "tb_papel_claims",
                newName: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "tb_papeis",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "tb_metas",
                newName: "Goals");

            migrationBuilder.RenameTable(
                name: "tb_limites_orcamento",
                newName: "BudgetLimits");

            migrationBuilder.RenameTable(
                name: "tb_categorias",
                newName: "Categories");

            migrationBuilder.RenameIndex(
                name: "IX_tb_usuario_papeis_RoleId",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_tb_usuario_logins_UserId",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_tb_usuario_claims_UserId",
                table: "AspNetUserClaims",
                newName: "IX_AspNetUserClaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_tb_papel_claims_RoleId",
                table: "AspNetRoleClaims",
                newName: "IX_AspNetRoleClaims_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_tb_limites_orcamento_UserId_Categoria",
                table: "BudgetLimits",
                newName: "IX_BudgetLimits_UserId_Categoria");

            migrationBuilder.RenameIndex(
                name: "IX_tb_categorias_UserId_Nome",
                table: "Categories",
                newName: "IX_Categories_UserId_Nome");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Goals",
                table: "Goals",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BudgetLimits",
                table: "BudgetLimits",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
