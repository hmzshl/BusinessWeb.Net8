using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BusinessWeb.Models;

namespace BusinessWeb.Data.Migrations
{
    public partial class CreateIdentitySchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            string q0 = @" IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'BFDB')
                              BEGIN
                                CREATE DATABASE [BFDB]";

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
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
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: false)
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
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
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
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
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,

                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

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
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

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

                           string q1 = @"CREATE TABLE [dbo].[T_Societe](
	                        [id] [int] IDENTITY(1,1) NOT NULL,
	                        [Intitule] [varchar](100) NOT NULL,
	                        [Ville] [varchar](100) NULL,
	                        [Adresse] [varchar](255) NULL,
	                        [Superficie] [decimal](27, 6) NOT NULL,
	                        [IdF] [varchar](100) NULL,
	                        [ICE] [varchar](100) NULL,
	                        [RC] [varchar](100) NULL,
	                        [CNSS] [varchar](100) NULL,
	                        [IdTVA] [varchar](100) NULL,
	                        [Patente] [varchar](100) NULL,
	                        [Capital] [decimal](27, 6) NOT NULL,
	                        [FormeJuridique] [int] NOT NULL,
	                        [Telephone] [varchar](100) NULL,
	                        [Fax] [varchar](100) NULL,
	                        [Email] [varchar](100) NULL,
	                        [Web] [varchar](100) NULL,
	                        [Abrege] [varchar](100) NULL,
	                        [DateDebut] [smalldatetime] NULL,
	                        [DateFin] [smalldatetime] NULL,
	                        [GestionCommercial] [bit] NOT NULL,
	                        [Rendement] [bit] NOT NULL,
	                        [RH] [bit] NOT NULL,
	                        [SousTraitance] [bit] NOT NULL,
	                        [Caisse] [bit] NOT NULL,
	                        [Banque] [bit] NOT NULL,
	                        [Recolte] [bit] NOT NULL,
	                        [RecolteSenegal] [bit] NOT NULL,
	                        [DateCreation] [smalldatetime] NULL,
	                        [Region] [int] NOT NULL,
	                        [Comptabilite] [bit] NOT NULL,
	                        [Tracabilite] [bit] NOT NULL,
	                        [Serveur] [varchar](100) NULL,
	                        [Base] [varchar](100) NULL,
	                        [Passe] [varchar](100) NULL,
                         CONSTRAINT [Pk_T_Societe_id] PRIMARY KEY CLUSTERED 
                        (
	                        [id] ASC
                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                        ) ON [PRIMARY]
                        GO

                        ALTER TABLE [dbo].[T_Societe] ADD  CONSTRAINT [defo_T_Societe_Superficie]  DEFAULT ((0)) FOR [Superficie]
                        GO

                        ALTER TABLE [dbo].[T_Societe] ADD  CONSTRAINT [defo_T_Societe_Capital]  DEFAULT ((0)) FOR [Capital]
                        GO

                        ALTER TABLE [dbo].[T_Societe] ADD  CONSTRAINT [defo_T_Societe_FormeJuridique]  DEFAULT ((0)) FOR [FormeJuridique]
                        GO

                        ALTER TABLE [dbo].[T_Societe] ADD  CONSTRAINT [defo_T_Societe_GestionCommercial]  DEFAULT ((0)) FOR [GestionCommercial]
                        GO

                        ALTER TABLE [dbo].[T_Societe] ADD  CONSTRAINT [defo_T_Societe_Rendement]  DEFAULT ((0)) FOR [Rendement]
                        GO

                        ALTER TABLE [dbo].[T_Societe] ADD  CONSTRAINT [defo_T_Societe_RH]  DEFAULT ((0)) FOR [RH]
                        GO

                        ALTER TABLE [dbo].[T_Societe] ADD  CONSTRAINT [defo_T_Societe_SousTraitance]  DEFAULT ((0)) FOR [SousTraitance]
                        GO

                        ALTER TABLE [dbo].[T_Societe] ADD  CONSTRAINT [defo_T_Societe_Caisse]  DEFAULT ((0)) FOR [Caisse]
                        GO

                        ALTER TABLE [dbo].[T_Societe] ADD  CONSTRAINT [defo_T_Societe_Banque]  DEFAULT ((0)) FOR [Banque]
                        GO

                        ALTER TABLE [dbo].[T_Societe] ADD  CONSTRAINT [defo_T_Societe_Recolte]  DEFAULT ((0)) FOR [Recolte]
                        GO

                        ALTER TABLE [dbo].[T_Societe] ADD  CONSTRAINT [defo_T_Societe_RecolteSenegal]  DEFAULT ((0)) FOR [RecolteSenegal]
                        GO

                        ALTER TABLE [dbo].[T_Societe] ADD  CONSTRAINT [defo_T_Societe_Region]  DEFAULT ((0)) FOR [Region]
                        GO

                        ALTER TABLE [dbo].[T_Societe] ADD  CONSTRAINT [defo_T_Societe_Comptabilite]  DEFAULT ((1)) FOR [Comptabilite]
                        GO

                        ALTER TABLE [dbo].[T_Societe] ADD  CONSTRAINT [defo_T_Societe_Tracabilite]  DEFAULT ((1)) FOR [Tracabilite]
                        GO
                            ";
            migrationBuilder.Sql(q1);

            string q2 = @"CREATE TABLE [dbo].[T_License](
	                        [id] [int] IDENTITY(1,1) NOT NULL,
	                        [Activation] [text] NULL,
                         CONSTRAINT [Pk_T_License_id] PRIMARY KEY CLUSTERED 
                        (
	                        [id] ASC
                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                        ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
            migrationBuilder.Sql(q2);

            string q3 = @"ALTER TABLE [dbo].[AspNetUsers] ADD [SelectedSociete] [int] NULL
                ALTER TABLE [dbo].[AspNetUsers] ADD [SelectedAPP] [int] NULL
                ALTER TABLE [dbo].[AspNetUsers] ADD [Intitule] [varchar](100) NULL";
            migrationBuilder.Sql(q3);

            migrationBuilder.Sql(RoleQuery("Admin", "0a63866c-0768-4aad-b1b4-ec5841de0001"));
            migrationBuilder.Sql(RoleQuery("Direction", "0a63866c-0768-4aad-b1b4-ec5841de0002"));
            migrationBuilder.Sql(RoleQuery("Commercial", "0a63866c-0768-4aad-b1b4-ec5841de0003"));
            migrationBuilder.Sql(RoleQuery("Acheteur", "0a63866c-0768-4aad-b1b4-ec5841de0004"));
            migrationBuilder.Sql(RoleQuery("Installateur", "0a63866c-0768-4aad-b1b4-ec5841de0005"));
            migrationBuilder.Sql(RoleQuery("Responsable RH", "0a63866c-0768-4aad-b1b4-ec5841de0006"));
            migrationBuilder.Sql(RoleQuery("Responsable Materiel", "0a63866c-0768-4aad-b1b4-ec5841de0007"));
            migrationBuilder.Sql(RoleQuery("Responsable Appels d''offre", "0a63866c-0768-4aad-b1b4-ec5841de0008"));
            migrationBuilder.Sql(RoleQuery("Responsable Marché", "0a63866c-0768-4aad-b1b4-ec5841de0009"));
            migrationBuilder.Sql(RoleQuery("Pointeur", "0a63866c-0768-4aad-b1b4-ec5841de0010"));
            migrationBuilder.Sql(RoleQuery("Magasinier", "0a63866c-0768-4aad-b1b4-ec5841de0011"));
            migrationBuilder.Sql(RoleQuery("Chef Chantier", "0a63866c-0768-4aad-b1b4-ec5841de0012"));
            migrationBuilder.Sql(RoleQuery("Chef Technique", "0a63866c-0768-4aad-b1b4-ec5841de0013"));
            migrationBuilder.Sql(RoleQuery("Assistant Chantier", "0a63866c-0768-4aad-b1b4-ec5841de0014"));
            migrationBuilder.Sql(RoleQuery("Caissier", "0a63866c-0768-4aad-b1b4-ec5841de0015"));
            migrationBuilder.Sql(RoleQuery("Comptable", "0a63866c-0768-4aad-b1b4-ec5841de0016"));
            migrationBuilder.Sql(RoleQuery("Responsable Site", "0a63866c-0768-4aad-b1b4-ec5841de0017"));
            migrationBuilder.Sql(RoleQuery("Agence Voyage", "0a63866c-0768-4aad-b1b4-ec5841de0018"));


            migrationBuilder.Sql(@"INSERT INTO AspNetUsers ([Id]
                                  ,[AccessFailedCount]
                                  ,[Email]
                                  ,[EmailConfirmed]
                                  ,[LockoutEnabled]
                                  ,[NormalizedEmail]
                                  ,[NormalizedUserName]
                                  ,[PasswordHash]
                                  ,[PhoneNumberConfirmed]
                                  ,[TwoFactorEnabled]
                                  ,[UserName]
                                  ,[Intitule]
                                   ,[SecurityStamp])

	                              VALUES (
	                              'a5f8e1a3-0bd5-437d-b4be-05a8019c4138',0,'Admin',1,1,'ADMIN','ADMIN','AQAAAAEAACcQAAAAEHl0prEIcGggHKsWPebf1hIZTaENGZpjw7/FetFMzqgyuY0nwa42Yppux+U/llECsQ==',0,0,'Admin','Admin','CGTOJLTBJIPQMDF3PQ4SN76XAFSHYA7N'
	                              )
                            ");

            migrationBuilder.Sql(@"INSERT INTO AspNetUserRoles (RoleId,UserId)

                                SELECT 
                                a.Id,b.Id
                                FROM AspNetRoles a 
                                INNER JOIN AspNetUsers b ON 1 = 1");

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
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
        private string RoleQuery(string role, string Id)
        {
            return @"INSERT INTO AspNetRoles (Id,Name,NormalizedName) VALUES ('"+Id+"','"+role+"','"+role.ToUpper()+"')";
        }
    }
}
