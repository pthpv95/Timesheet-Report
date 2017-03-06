using MediatR;
using System.Data.SqlClient;
using System.Linq;
using TimesheetReport.Core.Features.TRUsers;
using TimesheetReport.Core.Infrastructure.DAL;

namespace TimesheetReport.Core.Infrastructure.Handler.TRUsers
{
    public class GetListEmployeesQueryHandler : IRequestHandler<GetListEmployeesQuery, TRUser[]>
    {
        private readonly ManageUsersDbContext manageUsersDbContext;

        public GetListEmployeesQueryHandler(ManageUsersDbContext manageUsersDbContext)
        {
            this.manageUsersDbContext = manageUsersDbContext;
        }

        public TRUser[] Handle(GetListEmployeesQuery query)
        {

            var searchText = string.IsNullOrEmpty(query.searchText) ? "%%" : $"%{query.searchText}%";
            var rawQuery = @"
SELECT Id
      ,Status
      ,Email
      ,EmailConfirmed
      ,PasswordHash
      ,SecurityStamp
      ,PhoneNumber
      ,PhoneNumberConfirmed
      ,TwoFactorEnabled
      ,LockoutEndDateUtc
      ,LockoutEnabled
      ,AccessFailedCount
      ,UserName
      ,FirstName
      ,LastName
      ,EnglishName
      ,DOB
      ,IDNumber
      ,Position
      ,StartDate
      ,AvartarID
      ,Gender
        FROM Auth.AspNetUsers as P
        WHERE P.FirstName LIKE @searchText
         OR P.LastName LIKE @searchText;";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@searchText", searchText)
            };

            return manageUsersDbContext
                    .Set<TRUser>()
                    .SqlQuery(rawQuery, parameters)
                    .ToArray();
        }
    }
}