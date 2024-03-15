using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendPilketos.Migrations
{
    public partial class GroupVotesCountView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"create view GroupVoteCount as SELECT COUNT(Id) as Jml, JTB.Name FROM Votes
				LEFT JOIN (
						SELECT Users.id as UserId, 
						Users.Email, 
						Grp.Name,
						Grp.Id as GroupId
						FROM Users 
						LEFT JOIN UserGroups AS Grp ON Users.GroupId = Grp.id
					) AS JTB
				ON JTB.UserId = Votes.UserId
				GROUP BY JTB.Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				drop view GroupVoteCount;
			");
        }
    }
}
