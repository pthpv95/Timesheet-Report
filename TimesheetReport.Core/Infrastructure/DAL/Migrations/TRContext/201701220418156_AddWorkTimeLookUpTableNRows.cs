namespace TimesheetReport.Core.Infrastructure.DAL.Migrations.TRContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWorkTimeLookUpTableNRows : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "TR.WorkTimeLookUpTables",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ImportedTimeSheetDataFile_FileName = c.String(nullable: false),
                        ImportedTimeSheetDataFile_FileContent = c.Binary(nullable: false),
                        ImportedTimeSheetDataFile_ContentType = c.String(nullable: false, maxLength: 125),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "TR.WorkTimeLookUpTableRows",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        WorkTimeLookUpTableId = c.Guid(nullable: false),
                        ProjectName = c.String(maxLength: 250),
                        Task = c.String(maxLength: 250),
                        Hour = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("TR.WorkTimeLookUpTables", t => t.WorkTimeLookUpTableId, cascadeDelete: true)
                .Index(t => t.WorkTimeLookUpTableId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("TR.WorkTimeLookUpTableRows", "WorkTimeLookUpTableId", "TR.WorkTimeLookUpTables");
            DropIndex("TR.WorkTimeLookUpTableRows", new[] { "WorkTimeLookUpTableId" });
            DropTable("TR.WorkTimeLookUpTableRows");
            DropTable("TR.WorkTimeLookUpTables");
        }
    }
}
