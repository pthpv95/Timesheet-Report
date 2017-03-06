using System.Configuration;
using System.Data.SqlClient;

namespace TimesheetReport.Core.Infrastructure.DAL
{
    public interface ITimeSheetSqlConnectionFactory
    {
        SqlConnection Create();
    }
}       