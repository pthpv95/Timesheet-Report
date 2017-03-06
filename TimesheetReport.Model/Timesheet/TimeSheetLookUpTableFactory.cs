using System;

namespace TimesheetReport.Model.Timesheet
{
    public class TimeSheetLookUpTableFactory
    {
        public virtual WorkTimeLookUpTable CreateFromImportedData(TimeSheetData timeSheetData)
        {
            var workTimeLookUpTable = new WorkTimeLookUpTable(timeSheetData.TimeSheetDataFile);

            try
            {
                foreach (var timeSheetDataRow in timeSheetData.WorkTimeData.Rows)
                {
                    //var convertDateFromObjToDouble = Convert.ToDouble(timeSheetDataRow.Time);
                    //var date = DateTime.FromOADate(convertDateFromObjToDouble);

                    var workTimeLookUpTableRow = new WorkTimeLookUpTableRow()
                    {
                        WorkTimeLookUpTableId = workTimeLookUpTable.Id,
                        ProjectName = Convert.ToString(timeSheetDataRow.ProjectName),
                        Task = Convert.ToString(timeSheetDataRow.Task),
                        Hour = Convert.ToDouble(timeSheetDataRow.Hour),
                        Time = Convert.ToDateTime(timeSheetDataRow.Time)
                    };

                    workTimeLookUpTable.Rows.Add(workTimeLookUpTableRow);
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            return workTimeLookUpTable;
        }
    }
}