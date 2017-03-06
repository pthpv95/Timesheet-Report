using System;
using TimesheetReport.Model.Common;

namespace TimesheetReport.Model.Timesheet
{
    public class WorkTimeLookUpTableRow : Entity
    {
        public WorkTimeLookUpTableRow()
        {
            InitializeId();
        }

        public virtual WorkTimeLookUpTable Table { get; set; }

        public Guid WorkTimeLookUpTableId { get; set; }

        public string ProjectName { get; set; }

        public string Task { get; set; }

        public double Hour { get; set; }

        public DateTime Time { get; set; }
    }
}