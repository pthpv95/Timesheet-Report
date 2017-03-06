using AutoMapper;
using TimesheetReport.Model.Timesheet;

namespace TimesheetReport.Core.Infrastructure.Handler.TimeSheets
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile()
        {
            CreateMap<TimeSheetDataValidatorError, ImportTimeSheetDataFromExcelResultError>();
        }
    }
}