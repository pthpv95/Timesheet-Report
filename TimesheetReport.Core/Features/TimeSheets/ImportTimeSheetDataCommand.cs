using MediatR;
using TimesheetReport.Core.Infrastructure.Handler.TimeSheets;

namespace TimesheetReport.Core.Features.TimeSheets
{
    public class ImportTimeSheetDataCommand : IRequest<ImportTimeSheetDataFromExcelResult>
    {
        public string FileName { get; set; }

        public byte[] FileContent { get; set; }
    }
}