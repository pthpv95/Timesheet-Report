using System;
using TimesheetReport.Model.Timesheet;

namespace TimesheetReport.Core.Infrastructure.Handler.TimeSheets
{
    public interface ISaveWorkTimeTable
    {
        Guid Save(TimeSheetData timeSheetData);
    }
}