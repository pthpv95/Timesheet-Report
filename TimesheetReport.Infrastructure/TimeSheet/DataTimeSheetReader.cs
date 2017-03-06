using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using TimesheetReport.Core.Infrastructure.Handler.TimeSheets;
using TimesheetReport.Model.Timesheet;
using TimeSheetFile = TimesheetReport.Model.Common.Files;

namespace Timesheetreport.Infrastructure.TimeSheet
{
    public class DataTimeSheetReader : IDataTimeSheetReader
    {
        TimeSheetData IDataTimeSheetReader.Read(TimeSheetFile timeSheetDataFile)
        {
            using (ExcelPackage excelPackage = new ExcelPackage(new MemoryStream(timeSheetDataFile.FileContent)))
            {
                var workBook = excelPackage.Workbook;
                var workSheet = workBook.Worksheets["Timesheet"];

                var firstRow = 2;
                var lastRow = workSheet.Dimension.End.Row;

                var workTimeDataRows = new List<WorkTimeDataRow>();
                for (var rowIndex = firstRow; rowIndex <= lastRow; rowIndex++)
                {
                    var workTimeDataRow = new WorkTimeDataRow()
                    {
                        RowNumber = rowIndex,
                        ProjectName = workSheet.Cells["B" + rowIndex].Value,
                        Task = workSheet.Cells["C" + rowIndex].Value,
                        Hour = workSheet.Cells["D" + rowIndex].Value,
                        Time = workSheet.Cells["E" + rowIndex].Value
                    };

                    workTimeDataRows.Add(workTimeDataRow);
                }

                var timeSheetData = new TimeSheetData()
                {
                    TimeSheetDataFile = timeSheetDataFile,
                    WorkTimeData = new WorkTimeData() { Rows = workTimeDataRows.ToArray() }
                };

                return timeSheetData;
            }
        }
    }
}