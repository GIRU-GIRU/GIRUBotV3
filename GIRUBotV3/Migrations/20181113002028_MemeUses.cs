using Microsoft.EntityFrameworkCore.Migrations;

namespace GIRUBotV3.Migrations
{
    public partial class MemeUses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         //   migrationBuilder.AddColumn<int>(
          //      name: "MemeUses",
          //      table: "Memestore",
          //      nullable: false,
           //     defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemeUses",
                table: "Memestore");
        }
    }
}
