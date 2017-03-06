using System;

namespace TimesheetReport.Core.Infrastructure.Handler.TimeSheets
{
    public class ImportTimeSheetDataFromExcelResult
    {
        public bool HasError { get; set; }

        public ImportTimeSheetDataFromExcelResultError[] Errors { get; set; }

        public Guid TableId { get; set; }
    }

    public class ImportTimeSheetDataFromExcelResultError
    {
        public int? RowNumber { get; set; }
        public string ColumnName { get; set; }
        public string Error { get; set; }
    }
}