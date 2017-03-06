using AutoMapper;
using MediatR;
using System;
using System.Linq;
using TimesheetReport.Core.Features.TimeSheets;
using TimesheetReport.Core.Infrastructure.DAL;
using TimesheetReport.Model.Timesheet;
using TimeSheetFile = TimesheetReport.Model.Common.Files;
namespace TimesheetReport.Core.Infrastructure.Handler.TimeSheets
{
    public class ImportTimeSheetDataCommandHandler : IRequestHandler<ImportTimeSheetDataCommand, ImportTimeSheetDataFromExcelResult>
    {
        private readonly TimesheetReportContext timeSheetContext;
        private readonly IDataTimeSheetReader dataTimeSheetReader;
        private readonly TimeSheetDataValidator timeSheetDataValidator;
        private readonly ISaveWorkTimeTable saveWorkTimeTable;
        private readonly IMapper mapper;

        public ImportTimeSheetDataCommandHandler(
            TimesheetReportContext timeSheetContext,
            IDataTimeSheetReader dataTimeSheetReader,
            TimeSheetDataValidator timeSheetDataValidator,
            ISaveWorkTimeTable saveWorkTimeTable,
            IMapper mapper
            )
        {
            this.timeSheetContext = timeSheetContext;
            this.saveWorkTimeTable = saveWorkTimeTable;
            this.timeSheetDataValidator = timeSheetDataValidator;
            this.dataTimeSheetReader = dataTimeSheetReader;
            this.mapper = mapper;
        }

        public ImportTimeSheetDataFromExcelResult Handle(ImportTimeSheetDataCommand command)
        {
            TimeSheetData timeSheetData;

            try
            {
                var timeSheetDataFile = this.CreateTimeSheetFile(command.FileName, command.FileContent);
                timeSheetData = dataTimeSheetReader.Read(timeSheetDataFile);
            }
            catch
            {
                return FileReadError();
            }
            var validationErrors = timeSheetDataValidator.Validate(timeSheetData);

            if (validationErrors.Any())
            {
                return ValidationError(validationErrors);
            }
            try
            {
                var tableId = saveWorkTimeTable.Save(timeSheetData);
                return Success(tableId);
            }
            catch
            {
               return InsertWorkTimeError();
            }
        }

        private TimeSheetFile CreateTimeSheetFile(string fileName, byte[] fileContent)
        {
            return TimeSheetFile.Create(
                fileName,
                fileContent,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                );
        }

        private ImportTimeSheetDataFromExcelResult ValidationError(TimeSheetDataValidatorError[] validationErrors)
        {
            return new ImportTimeSheetDataFromExcelResult
            {
                HasError = true,
                Errors = mapper.Map<ImportTimeSheetDataFromExcelResultError[]>(validationErrors)
            };
        }
        private static ImportTimeSheetDataFromExcelResult FileReadError()
        {
            return new ImportTimeSheetDataFromExcelResult
            {
                HasError = true,
                Errors = new[]
                {
                    new ImportTimeSheetDataFromExcelResultError {Error = "Invalid format file" }
                }
            };
        }

        private static ImportTimeSheetDataFromExcelResult Success(Guid tableId)
        {
            return new ImportTimeSheetDataFromExcelResult
            {
                HasError = false,
                TableId = tableId
            };
        }

        private static ImportTimeSheetDataFromExcelResult InsertWorkTimeError()
        {
            return new ImportTimeSheetDataFromExcelResult
            {
                HasError = true,
                Errors = new[]
                {
                    new ImportTimeSheetDataFromExcelResultError {Error = "Insert workTime to database failed" }
                }
            };
        }
    }
}