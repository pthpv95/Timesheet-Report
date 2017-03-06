using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using TimesheetReport.Core.Infrastructure.DAL;
using TimesheetReport.Core.Infrastructure.Handler.TimeSheets;
using TimesheetReport.Model.Timesheet;

namespace Timesheetreport.Infrastructure.TimeSheet
{
    public class SaveWorkTimeLookUpTable : ISaveWorkTimeTable
    {
        private readonly ITimeSheetSqlConnectionFactory connectionFactory;

        private readonly TimeSheetLookUpTableFactory timeSheetLookUpTableFactory;

        private DataTable workTimeLookUpDataTable;
        private DataTable workTimeLookUpDataTableRow;

        private static readonly string[] WorkTimeLookDataTableRowColumns = new string[]
        {
            "Id",
            "WorkTimeLookUpTableId",
            "ProjectName",
            "Task",
            "Hour",
            "Time"
        };

        public SaveWorkTimeLookUpTable(ITimeSheetSqlConnectionFactory connectionFactory,
            TimeSheetLookUpTableFactory timeSheetLookUpTableFactory)
        {
            this.connectionFactory = connectionFactory;
            this.timeSheetLookUpTableFactory = timeSheetLookUpTableFactory;

            InitializeWorkTimeLookUpTable();
            InitializeWorkTimeLookUpTableRow();
        }
        Guid ISaveWorkTimeTable.Save(TimeSheetData timeSheetData)
        {
            var workTimeLookUpTable = timeSheetLookUpTableFactory.CreateFromImportedData(timeSheetData);

            using (var connection = connectionFactory.Create())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        LoadWorkTimeDataToDataTable(workTimeLookUpTable);

                        InsertWorkTimeLookUpTables(connection, transaction);
                        InsertWorkTimeLookUpTableRows(connection, transaction);

                        transaction.Commit();

                        return workTimeLookUpTable.Id;
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        private void InsertWorkTimeLookUpTables(SqlConnection connection, SqlTransaction transaction)
        {
            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.CheckConstraints, transaction))
            {
                sqlBulkCopy.BulkCopyTimeout = 600;
                sqlBulkCopy.DestinationTableName = "TR.WorkTimeLookUpTables";

                sqlBulkCopy.ColumnMappings.Add("Id", "Id");
                sqlBulkCopy.ColumnMappings.Add("CreatedOn", "CreatedOn");
                sqlBulkCopy.ColumnMappings.Add("ImportedTimeSheetDataFile_FileName", "ImportedTimeSheetDataFile_FileName");
                sqlBulkCopy.ColumnMappings.Add("ImportedTimeSheetDataFile_FileContent", "ImportedTimeSheetDataFile_FileContent");
                sqlBulkCopy.ColumnMappings.Add("ImportedTimeSheetDataFile_ContentType", "ImportedTimeSheetDataFile_ContentType");

                sqlBulkCopy.WriteToServer(workTimeLookUpDataTable);
            }
        }
        private void InsertWorkTimeLookUpTableRows(SqlConnection connection,SqlTransaction transaction)
        {
            using(SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.CheckConstraints, transaction))
            {
                sqlBulkCopy.BulkCopyTimeout = 600;
                sqlBulkCopy.DestinationTableName = "TR.WorkTimeLookUpTableRows";

                sqlBulkCopy.ColumnMappings.Add("Id", "Id");
                sqlBulkCopy.ColumnMappings.Add("WorkTimeLookUpTableId", "WorkTimeLookUpTableId");
                sqlBulkCopy.ColumnMappings.Add("ProjectName", "ProjectName");
                sqlBulkCopy.ColumnMappings.Add("Task", "Task");
                sqlBulkCopy.ColumnMappings.Add("Hour", "Hour");
                sqlBulkCopy.ColumnMappings.Add("Time", "Time");

                sqlBulkCopy.WriteToServer(workTimeLookUpDataTableRow);
            }
        }

        private void InitializeWorkTimeLookUpTable()
        {
            workTimeLookUpDataTable = new DataTable("WorkTimeLookUpTableBulkInsert");
            workTimeLookUpDataTable.Columns.Add("Id", typeof(Guid));
            workTimeLookUpDataTable.Columns.Add("CreatedOn", typeof(DateTime));
            workTimeLookUpDataTable.Columns.Add("ImportedTimeSheetDataFile_FileName", typeof(string));
            workTimeLookUpDataTable.Columns.Add("ImportedTimeSheetDataFile_FileContent", typeof(byte[]));
            workTimeLookUpDataTable.Columns.Add("ImportedTimeSheetDataFile_ContentType", typeof(string));
            workTimeLookUpDataTable.Columns.Add("Time", typeof(DateTime));
        }

        private void InitializeWorkTimeLookUpTableRow()
        {
            var workTimeLookUpTableRowProperties = typeof(WorkTimeLookUpTableRow).GetProperties();

            workTimeLookUpDataTableRow = new DataTable("WorkTimeLookTableRowBulkInsert");
            foreach (PropertyInfo property in workTimeLookUpTableRowProperties)
            {
                if (WorkTimeLookDataTableRowColumns.Contains(property.Name))
                {
                    workTimeLookUpDataTableRow.Columns.Add(property.Name, property.PropertyType);
                }
            }
        }

        private void LoadWorkTimeDataToDataTable(WorkTimeLookUpTable workTimeLookUpTable)
        {
            LoadWorkTimeLookUpTable(workTimeLookUpTable);
            LoadWorkTimeLookUpTableRow(workTimeLookUpTable.Rows);
        }
        private void LoadWorkTimeLookUpTable(WorkTimeLookUpTable workTimeLookUpTable)
        {
            var workTimeLookUpTableRow = workTimeLookUpDataTable.NewRow();

            workTimeLookUpTableRow["Id"] = workTimeLookUpTable.Id;
            workTimeLookUpTableRow["CreatedOn"] = workTimeLookUpTable.CreatedOn;
            workTimeLookUpTableRow["ImportedTimeSheetDataFile_FileName"] = workTimeLookUpTable.ImportedTimeSheetDataFile.FileName;
            workTimeLookUpTableRow["ImportedTimeSheetDataFile_FileContent"] = workTimeLookUpTable.ImportedTimeSheetDataFile.FileContent;
            workTimeLookUpTableRow["ImportedTimeSheetDataFile_ContentType"] = workTimeLookUpTable.ImportedTimeSheetDataFile.ContentType;

            workTimeLookUpDataTable.Rows.Add(workTimeLookUpTableRow);
        }

        private void LoadWorkTimeLookUpTableRow(ICollection<WorkTimeLookUpTableRow> workTimeLookUpTableRows)
        {
            var workTimeLookUpTableRowType = typeof(WorkTimeLookUpTableRow);

            foreach (var workTimeLookUpTableRow in workTimeLookUpTableRows)
            {
                var newRow = workTimeLookUpDataTableRow.NewRow();
                foreach (var column in WorkTimeLookDataTableRowColumns)
                {
                    var property = workTimeLookUpTableRowType.GetProperty(column);
                    newRow[column] = property.GetValue(workTimeLookUpTableRow);
                }

                workTimeLookUpDataTableRow.Rows.Add(newRow);
            }
        }
    }
}