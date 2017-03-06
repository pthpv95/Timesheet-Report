namespace TimesheetReport.Core.Infrastructure.DAL.Migrations.TRContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTimeColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("TR.WorkTimeLookUpTableRows", "Time", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("TR.WorkTimeLookUpTableRows", "Time");
        }
    }
}
