using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Notifications.DataAccess.Migrations
{
	public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Template = table.Column<string>(maxLength: 1024, nullable: false),
                    Type = table.Column<string>(maxLength: 128, nullable: false),
                    TypeName = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.Id);
                });

            //migrationBuilder.CreateTable(
            //    name: "Users",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(nullable: false),
            //        FirstName = table.Column<string>(nullable: false),
            //        OrganisationName = table.Column<string>(nullable: true),
            //        Email = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Users", x => x.Id);
            //    });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AppointmentDateTime = table.Column<DateTime>(nullable: true),
                    NotificationTypeId = table.Column<Guid>(nullable: true), 
                    UserId = table.Column<Guid>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    OrganisationName = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    MessageContent = table.Column<string>(maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_NotificationTypes_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "NotificationTypes",
                        principalColumn: "Id");
                    //table.ForeignKey(
                    //    name: "FK_Notifications_Users_UserId", 
                    //    column: x => x.UserId, 
                    //    principalTable: "Users", 
                    //    principalColumn: "Id");
                });

            migrationBuilder.Sql("INSERT INTO NotificationTypes (Id, Template, Type, TypeName) VALUES " + 
                "('" + Guid.NewGuid() + "', 'Id: {0}\nEventType: {1}\nBody: Hi {2}, your appointment with {3} at {4} has \nbeen - cancelled for the following reason: {5}.\nTitle: {6}', 'AppointmentCancelled', 'Appointment Cancelled')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationTypes");

            migrationBuilder.DropTable(
                name: "Notifications");
        }
    }
}