using System.Configuration;
using System.Data.SqlClient;

namespace TimesheetReport.Core.Infrastructure.DAL
{
    public class TimeSheetSqlConnectionFactory : ITimeSheetSqlConnectionFactory
    {
        private readonly string connectionStringName;

        public TimeSheetSqlConnectionFactory(string connectionStringName)
        {
            this.connectionStringName = connectionStringName;
        }

        SqlConnection ITimeSheetSqlConnectionFactory.Create()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            return new SqlConnection(connectionString);
        }
    }
}