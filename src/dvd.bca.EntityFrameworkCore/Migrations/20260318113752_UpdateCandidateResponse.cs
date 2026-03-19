using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dvd.bca.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCandidateResponse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_candidateResponses_Applications_applicationId",
                table: "candidateResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_candidateResponses_offers_offerId",
                table: "candidateResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_candidateResponses",
                table: "candidateResponses");

            migrationBuilder.RenameTable(
                name: "candidateResponses",
                newName: "CandidateResponses");

            migrationBuilder.RenameColumn(
                name: "responseType",
                table: "CandidateResponses",
                newName: "ResponseType");

            migrationBuilder.RenameColumn(
                name: "responseTime",
                table: "CandidateResponses",
                newName: "ResponseTime");

            migrationBuilder.RenameColumn(
                name: "offerId",
                table: "CandidateResponses",
                newName: "OfferId");

            migrationBuilder.RenameColumn(
                name: "applicationId",
                table: "CandidateResponses",
                newName: "ApplicationId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "CandidateResponses",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_candidateResponses_offerId",
                table: "CandidateResponses",
                newName: "IX_CandidateResponses_OfferId");

            migrationBuilder.RenameIndex(
                name: "IX_candidateResponses_applicationId",
                table: "CandidateResponses",
                newName: "IX_CandidateResponses_ApplicationId");

            migrationBuilder.AlterColumn<int>(
                name: "ResponseType",
                table: "CandidateResponses",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "CandidateResponses",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OfferId1",
                table: "CandidateResponses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResponseChannel",
                table: "CandidateResponses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ResponseContent",
                table: "CandidateResponses",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CandidateResponses",
                table: "CandidateResponses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateResponses_OfferId1",
                table: "CandidateResponses",
                column: "OfferId1");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateResponses_ResponseTime",
                table: "CandidateResponses",
                column: "ResponseTime");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateResponses_ResponseType",
                table: "CandidateResponses",
                column: "ResponseType");

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateResponses_Applications_ApplicationId",
                table: "CandidateResponses",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateResponses_offers_OfferId",
                table: "CandidateResponses",
                column: "OfferId",
                principalTable: "offers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateResponses_offers_OfferId1",
                table: "CandidateResponses",
                column: "OfferId1",
                principalTable: "offers",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidateResponses_Applications_ApplicationId",
                table: "CandidateResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_CandidateResponses_offers_OfferId",
                table: "CandidateResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_CandidateResponses_offers_OfferId1",
                table: "CandidateResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CandidateResponses",
                table: "CandidateResponses");

            migrationBuilder.DropIndex(
                name: "IX_CandidateResponses_OfferId1",
                table: "CandidateResponses");

            migrationBuilder.DropIndex(
                name: "IX_CandidateResponses_ResponseTime",
                table: "CandidateResponses");

            migrationBuilder.DropIndex(
                name: "IX_CandidateResponses_ResponseType",
                table: "CandidateResponses");

            migrationBuilder.DropColumn(
                name: "OfferId1",
                table: "CandidateResponses");

            migrationBuilder.DropColumn(
                name: "ResponseChannel",
                table: "CandidateResponses");

            migrationBuilder.DropColumn(
                name: "ResponseContent",
                table: "CandidateResponses");

            migrationBuilder.RenameTable(
                name: "CandidateResponses",
                newName: "candidateResponses");

            migrationBuilder.RenameColumn(
                name: "ResponseType",
                table: "candidateResponses",
                newName: "responseType");

            migrationBuilder.RenameColumn(
                name: "ResponseTime",
                table: "candidateResponses",
                newName: "responseTime");

            migrationBuilder.RenameColumn(
                name: "OfferId",
                table: "candidateResponses",
                newName: "offerId");

            migrationBuilder.RenameColumn(
                name: "ApplicationId",
                table: "candidateResponses",
                newName: "applicationId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "candidateResponses",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_CandidateResponses_OfferId",
                table: "candidateResponses",
                newName: "IX_candidateResponses_offerId");

            migrationBuilder.RenameIndex(
                name: "IX_CandidateResponses_ApplicationId",
                table: "candidateResponses",
                newName: "IX_candidateResponses_applicationId");

            migrationBuilder.AlterColumn<string>(
                name: "responseType",
                table: "candidateResponses",
                type: "nvarchar(100)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "candidateResponses",
                type: "nvarchar(1000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddPrimaryKey(
                name: "PK_candidateResponses",
                table: "candidateResponses",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_candidateResponses_Applications_applicationId",
                table: "candidateResponses",
                column: "applicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_candidateResponses_offers_offerId",
                table: "candidateResponses",
                column: "offerId",
                principalTable: "offers",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
