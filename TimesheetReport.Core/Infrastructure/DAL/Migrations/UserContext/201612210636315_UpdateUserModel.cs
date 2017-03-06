namespace TimesheetReport.Core.Infrastructure.DAL.Migrations.UserContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("Auth.AspNetUsers", "Gender", c => c.Int(nullable: false));
            AlterColumn("Auth.AspNetUsers", "DOB", c => c.DateTime(nullable: false));
            AlterColumn("Auth.AspNetUsers", "AvartarId", c => c.Guid());
            DropColumn("Auth.AspNetUsers", "Genders");
        }
        
        public override void Down()
        {
            AddColumn("Auth.AspNetUsers", "Genders", c => c.Int(nullable: false));
            AlterColumn("Auth.AspNetUsers", "AvartarId", c => c.Guid(nullable: false));
            AlterColumn("Auth.AspNetUsers", "DOB", c => c.DateTime());
            DropColumn("Auth.AspNetUsers", "Gender");
        }
    }
}
