using AutoMapper;
using TimesheetReport.WebUI.Models;
using TimesheetReport.WebUI.ViewModels;

namespace TimesheetReport.WebUI.Mappings
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile()
        {
            CreateMap<ListTimeSheet, TimeSheetViewModel>();
        }
    }
}