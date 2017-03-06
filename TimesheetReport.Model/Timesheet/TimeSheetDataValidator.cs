using System.Collections.Generic;

namespace TimesheetReport.Model.Timesheet
{
    public class TimeSheetDataValidator
    {
        private readonly WorkTimeDataValidator workTimeDataValidator;

        public TimeSheetDataValidator(WorkTimeDataValidator workTimeDataValidator)
        {
            this.workTimeDataValidator = workTimeDataValidator;
        }

        public virtual TimeSheetDataValidatorError[] Validate(TimeSheetData timeSheetData)
        {
            var timeSheetDataValidationResult = new List<TimeSheetDataValidatorError>();
            var workTimeDataValidationResult = workTimeDataValidator.Validate(timeSheetData.WorkTimeData);

            timeSheetDataValidationResult.AddRange(workTimeDataValidationResult);

            return timeSheetDataValidationResult.ToArray();
        }
    }

    public class TimeSheetDataValidatorError
    {
        public int? RowNumber { get; set; }

        public string ColumnName { get; set; }

        public string Error { get; set; }
    }
}