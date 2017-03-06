namespace TimesheetReport.Core.Infrastructure.DAL.Migrations.UserContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGenders : DbMigration
    {
        public override void Up()
        {
            AddColumn("Auth.AspNetUsers", "Genders", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Auth.AspNetUsers", "Genders");
        }
    }
}
