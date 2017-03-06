using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimesheetReport.Core.Features.TRUsers;

namespace TimesheetReport.WebUI.ViewModels.Employee
{
    public class Employee
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Genders Genders { get; set; }
    }
}