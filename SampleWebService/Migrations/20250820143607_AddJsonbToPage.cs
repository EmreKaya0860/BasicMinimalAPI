using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SampleWebService.Migrations
{
    /// <inheritdoc />
    public partial class AddJsonbToPage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Mevcut Data kolonunu jsonb'ye dönüştür
            migrationBuilder.Sql(
                @"ALTER TABLE ""Pages"" 
                  ALTER COLUMN ""Data"" TYPE jsonb USING ""Data""::jsonb;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // jsonb'yi tekrar text'e dönüştür
            migrationBuilder.Sql(
                @"ALTER TABLE ""Pages"" 
                  ALTER COLUMN ""Data"" TYPE text USING ""Data""::text;");
        }
    }
}
