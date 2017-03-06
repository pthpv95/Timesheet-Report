namespace TimesheetReport.Core.Infrastructure.DAL.Migrations.TRContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class makeIsActiveOptional : DbMigration
    {
        public override void Up()
        {
            AlterColumn("TR.WorkTimeLookUpTables", "IsActive", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("TR.WorkTimeLookUpTables", "IsActive", c => c.Boolean(nullable: false));
        }
    }
}
