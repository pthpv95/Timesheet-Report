using System;
using System.Collections.Generic;
using TimesheetReport.Model.Common;

namespace TimesheetReport.Model.Timesheet
{
    public class WorkTimeLookUpTable : Entity
    {
        protected WorkTimeLookUpTable()
        {
        }

        public WorkTimeLookUpTable(Files timeSheetDataFile)
        {
            if (timeSheetDataFile == null)
            {
                throw new ArgumentNullException("timeSheetDataFile");
            }
            InitializeId();
            CreatedOn = DateTime.Now;
            ImportedTimeSheetDataFile = timeSheetDataFile;
            Rows = new List<WorkTimeLookUpTableRow>();
        }

        public DateTime CreatedOn { get; private set; }

        public Files ImportedTimeSheetDataFile { get;private set; }

        public virtual ICollection<WorkTimeLookUpTableRow> Rows { get; private set; }

        public bool IsActive { get; set; }

        public void Activate()
        {
            IsActive = true;
        }

        public void InActivate()
        {
            IsActive = false;
        }
    }
}