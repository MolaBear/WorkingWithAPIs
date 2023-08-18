using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkingWithAPIs.Models;
using WorkingWithAPIs.Models.ViewModel;

namespace WorkingWithAPIs.Data
{
    public class WorkingWithAPIsContext : DbContext
    {
        public WorkingWithAPIsContext (DbContextOptions<WorkingWithAPIsContext> options)
            : base(options)
        {
        }

        public DbSet<UsersModel> Users { get; set; } = default!;

        public List<UserViewModel> MapUsersToView(List<UsersModel> usersList)
        {
            return usersList.Select(usersInfo => new UserViewModel
            {
                firstName = usersInfo.firstName,
                lastName = usersInfo.lastName,
                email = usersInfo.email,
                telephone = usersInfo.telephone,
                identityNumber = usersInfo.identityNumber
        }).ToList();
        }
    }
}
