using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TimesheetReport.Model.Timesheet
{
    public class WorkTimeDataValidator
    {
        private readonly int maxErrorCount = 15;
        private List<TimeSheetDataValidatorError> errors;
        private string[] acceptedProject;

        public WorkTimeDataValidator()
        {
            acceptedProject = new string[]
            {
                "SingLife Comparison",
                "Stark",
                "Guppi Box",
                "Pikachu"
            };
        }
        public virtual TimeSheetDataValidatorError[] Validate(WorkTimeData workTimeData)
        {
            errors = new List<TimeSheetDataValidatorError>();

            try
            {
                ValidateWorkTimeData(workTimeData.Rows);
            }
            catch
            {
            }
            return errors.ToArray();
        }

        private void ValidateWorkTimeData(WorkTimeDataRow[] workTimeDataRows)
        {
            var propertyInfo = GetWorkTimeDataRowPropertiesInfo();

            foreach (var row in workTimeDataRows)
            {
                ValidateWorkTimeDataRow(row, propertyInfo);
            }
        }

        private void ValidateWorkTimeDataRow(WorkTimeDataRow row, PropertyInfo[] propertiesInfo)
        {
            foreach (var property in propertiesInfo)
            {
                var valueObject = property.GetValue(row);
                if (IsConvertibleToNotNullOrEmptyObject(valueObject))
                {
                    ValidateField(row, property, valueObject);
                }
                else
                {
                    AddError("Value is empty or consists only white-space characters.", row.RowNumber, property.Name);
                }
            }
        }

        private void ValidateField(WorkTimeDataRow row,PropertyInfo property,object objectValue)
        {
            var propertyName = property.Name;

            switch (propertyName)
            {
                case "ProjectName":
                    string projectName = Convert.ToString(objectValue);
                    projectName = projectName.Trim();

                    if (acceptedProject.Contains(projectName) == false)
                    {
                        AddError(string.Format("ProjectName is not accepted.Just accept{0}", string.Join(",", acceptedProject)),row.RowNumber,"ProjectName");
                    }
                    break;
                case "Hour":
                    try
                    {
                        int hour = Convert.ToInt32(objectValue);
                        if (hour > 8)
                        {
                            AddError("Hour cannot be greater than 8", row.RowNumber, "Hour");
                        }
                    }
                    catch
                    {
                        AddError("Hour is not a number", row.RowNumber, "Hour");
                    }
                    break;
                //case "Time":
                //    DateTime time;
                //    string valueDate = Convert.ToString(objectValue);
                //    if(!DateTime.TryParse(valueDate, out time))
                //    {
                //        AddError("Invalid time", row.RowNumber, "Time");
                //    }
                //    break;
            }

        }

        private PropertyInfo[] GetWorkTimeDataRowPropertiesInfo()
        {
            return typeof(WorkTimeDataRow).GetProperties();
        }

        private bool IsConvertibleToNotNullOrEmptyObject(object valueObject)
        {
            var stringValue = Convert.ToString(valueObject);

            if (string.IsNullOrEmpty(stringValue))
            {
                return false;
            }
            return true;
        }

        private void AddError(string errorMessage,int? rowNumber, string columnName= null)
        {
            AddErrors(new TimeSheetDataValidatorError
            {
                RowNumber = rowNumber,
                Error = errorMessage,
                ColumnName = columnName
            });
        }

        private void AddErrors( params TimeSheetDataValidatorError[] errors)
        {
            this.errors.AddRange(errors);

            if(this.errors.Count > maxErrorCount)
            {
                this.errors.Add(new TimeSheetDataValidatorError
                {
                    Error = "Too many errors."
                });

                throw new Exception();
            }
        }
    }

   
}