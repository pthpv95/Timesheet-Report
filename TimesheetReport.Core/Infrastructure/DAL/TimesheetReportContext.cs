using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using TimesheetReport.Core.Features.Files;
using TimesheetReport.Core.Features.MyEquipment;
using TimesheetReport.Core.Features.Projects;
using TimesheetReport.Core.Features.TimeSheets;
using TimesheetReport.Model.Timesheet;

namespace TimesheetReport.Core.Infrastructure.DAL
{
    public class TimesheetReportContext : DbContext
    {
        public TimesheetReportContext()
            : base("TimesheetReportContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public override int SaveChanges()
        {
            RaiseDomainEvents();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            RaiseDomainEvents();
            return base.SaveChangesAsync();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            RaiseDomainEvents();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void RaiseDomainEvents()
        {
            // TODO: Raise domain event that were registered with entities.
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Database.SetInitializer<TimesheetReportContext>(null);

            modelBuilder.HasDefaultSchema("TR");
            modelBuilder.Entity<File>().ToTable("Files");

            modelBuilder.Entity<Project>().ToTable("Projects");
            modelBuilder.Entity<Project>().HasKey(x => x.Id);

            modelBuilder.Entity<WorkTime>().ToTable("WorkTimes");
            modelBuilder.Entity<WorkTime>().HasKey(x => x.TimeId);

            modelBuilder.Entity<Equipment>().ToTable("Equipments");

            MapWorkTimeLookUpTable(modelBuilder);
            MapWorkTimeLookUpTableRow(modelBuilder);
        }

        private void MapWorkTimeLookUpTable(DbModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<WorkTimeLookUpTable>();

            entity
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            entity
                .HasMany(e => e.Rows)
                .WithRequired(r => r.Table)
                .HasForeignKey(k => k.WorkTimeLookUpTableId);

            entity
                .Property(e => e.ImportedTimeSheetDataFile.FileContent)
                .IsRequired();

            entity
                .Property(e => e.ImportedTimeSheetDataFile.FileName)
                .IsRequired();

            entity
                .Property(e => e.ImportedTimeSheetDataFile.ContentType)
                .HasMaxLength(125)
                .IsRequired();

            entity
                .Property(e => e.IsActive)
                .IsOptional();
        }

        private void MapWorkTimeLookUpTableRow(DbModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<WorkTimeLookUpTableRow>();

            entity
                .Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            entity
                .Property(x => x.ProjectName)
                .HasMaxLength(250);

            entity
                .Property(x => x.Task)
                .HasMaxLength(250);

            entity
                .Property(x => x.Hour)
                .IsRequired();

            entity
                .Property(e => e.Time)
                .IsRequired();
        }
    }
}