using TimesheetReport.Model.Timesheet;
using TimesheetReport.Model.Common;
namespace TimesheetReport.Core.Infrastructure.Handler.TimeSheets
{
    public interface IDataTimeSheetReader
    {
        TimeSheetData Read(Model.Common.Files workTimeData);
    }
}