using System;

namespace TimesheetReport.Model.Timesheet
{
    public class WorkTimeData
    {
        public WorkTimeDataRow[] Rows { get; set; }
    }

    public class WorkTimeDataRow
    {
        public int RowNumber { get; set; }

        public object ProjectName { get; set; }

        public object Task { get; set; }

        public object Hour { get; set; }

        public object Time { get; set; }

    }
}