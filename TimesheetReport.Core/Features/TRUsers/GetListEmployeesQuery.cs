using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimesheetReport.Core.Features.TRUsers
{
    public class GetListEmployeesQuery:IRequest<TRUser[]>
    {
        public string searchText { get; set; }
    }
}
