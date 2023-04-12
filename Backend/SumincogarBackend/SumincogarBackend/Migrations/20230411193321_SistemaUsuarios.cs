using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SumincogarBackend.Migrations
{
    public partial class SistemaUsuarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CATALOGO",
                columns: table => new
                {
                    CATALOGOID = table.Column<int>(type: "int", nullable: false),
                    NOMBRE = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    CATALOGOURL = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    IMAGENREFERENCIAL = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CATALOGO", x => x.CATALOGOID);
                });

            migrationBuilder.CreateTable(
                name: "CATEGORIA",
                columns: table => new
                {
                    CATEGORIAID = table.Column<int>(type: "int", nullable: false),
                    CATEGORIANOMBRE = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CATEGORIA", x => x.CATEGORIAID);
                });

            migrationBuilder.CreateTable(
                name: "DETALLEINVENTARIO",
                columns: table => new
                {
                    DETALLEINVENTARIOID = table.Column<int>(type: "int", nullable: false),
                    CODCLIENTE = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    CODPRODUCTO = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    STOCK = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    IMPRESION = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    COLORES = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DETALLEINVENTARIO", x => x.DETALLEINVENTARIOID);
                });

            migrationBuilder.CreateTable(
                name: "PROMOCION",
                columns: table => new
                {
                    PROMOCIONID = table.Column<int>(type: "int", nullable: false),
                    TITULO = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    FECHAINGRESO = table.Column<DateTime>(type: "datetime", nullable: false),
                    FECHACADUCIDAD = table.Column<DateTime>(type: "datetime", nullable: false),
                    PRIORIDAD = table.Column<bool>(type: "bit", nullable: false),
                    IMAGENPRINCIPAL = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROMOCION", x => x.PROMOCIONID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCTO",
                columns: table => new
                {
                    PRODUCTOID = table.Column<int>(type: "int", nullable: false),
                    CATEGORIAID = table.Column<int>(type: "int", nullable: true),
                    CODIGO = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    PRODUCTONOMBRE = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    FICHATENICAPDF = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    IMAGENREFERENCIAL = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCTO", x => x.PRODUCTOID);
                    table.ForeignKey(
                        name: "FK_PRODUCTO_RELATIONS_CATEGORI",
                        column: x => x.CATEGORIAID,
                        principalTable: "CATEGORIA",
                        principalColumn: "CATEGORIAID");
                });

            migrationBuilder.CreateTable(
                name: "PROMOCIONIMAGEN",
                columns: table => new
                {
                    PROMOCIONIMAGENID = table.Column<int>(type: "int", nullable: false),
                    PROMOCIONID = table.Column<int>(type: "int", nullable: true),
                    PROMOCIONIMAGENURL = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    ORDEN = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROMOCIONIMAGEN", x => x.PROMOCIONIMAGENID);
                    table.ForeignKey(
                        name: "FK_PROMOCIO_RELATIONS_PROMOCIO",
                        column: x => x.PROMOCIONID,
                        principalTable: "PROMOCION",
                        principalColumn: "PROMOCIONID");
                });

            migrationBuilder.CreateTable(
                name: "IMAGENREFERENCIAL",
                columns: table => new
                {
                    IMAGENREFERENCIALID = table.Column<int>(type: "int", nullable: false),
                    PRODUCTOID = table.Column<int>(type: "int", nullable: true),
                    IMAGENREFERENCIALURL = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMAGENREFERENCIAL", x => x.IMAGENREFERENCIALID);
                    table.ForeignKey(
                        name: "FK_IMAGENRE_RELATIONS_PRODUCTO",
                        column: x => x.PRODUCTOID,
                        principalTable: "PRODUCTO",
                        principalColumn: "PRODUCTOID");
                });

            migrationBuilder.CreateTable(
                name: "PARAMETROTECNICO",
                columns: table => new
                {
                    PARAMETROTECNICOID = table.Column<int>(type: "int", nullable: false),
                    PRODUCTOID = table.Column<int>(type: "int", nullable: true),
                    CLAVE = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    VALOR = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PARAMETROTECNICO", x => x.PARAMETROTECNICOID);
                    table.ForeignKey(
                        name: "FK_PARAMETR_RELATIONS_PRODUCTO",
                        column: x => x.PRODUCTOID,
                        principalTable: "PRODUCTO",
                        principalColumn: "PRODUCTOID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "RELATIONSHIP_2_FK",
                table: "IMAGENREFERENCIAL",
                column: "PRODUCTOID");

            migrationBuilder.CreateIndex(
                name: "RELATIONSHIP_3_FK",
                table: "PARAMETROTECNICO",
                column: "PRODUCTOID");

            migrationBuilder.CreateIndex(
                name: "RELATIONSHIP_1_FK",
                table: "PRODUCTO",
                column: "CATEGORIAID");

            migrationBuilder.CreateIndex(
                name: "RELATIONSHIP_4_FK",
                table: "PROMOCIONIMAGEN",
                column: "PROMOCIONID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CATALOGO");

            migrationBuilder.DropTable(
                name: "DETALLEINVENTARIO");

            migrationBuilder.DropTable(
                name: "IMAGENREFERENCIAL");

            migrationBuilder.DropTable(
                name: "PARAMETROTECNICO");

            migrationBuilder.DropTable(
                name: "PROMOCIONIMAGEN");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "PRODUCTO");

            migrationBuilder.DropTable(
                name: "PROMOCION");

            migrationBuilder.DropTable(
                name: "CATEGORIA");
        }
    }
}
