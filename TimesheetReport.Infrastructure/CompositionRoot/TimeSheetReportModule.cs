using Autofac;
using TimesheetReport.Core.Infrastructure.DAL;

namespace TimesheetReport.Infrastructure.CompositionRoot
{
    public class TimeSheetReportModule : Module
    {
        public string TimeSheetReportConnectionStringName { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            RegisterDALComponents(builder);
        }

        private void RegisterDALComponents(ContainerBuilder builder)
        {
            builder
                .RegisterType<TimeSheetSqlConnectionFactory>()
                .AsImplementedInterfaces()
                .WithParameter((pi, ctx) => pi.ParameterType == typeof(string), (pi, ctx) => TimeSheetReportConnectionStringName);
        }
    }
}