using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCPrbSol.Models.ViewModels
{
    public class ManageProjectUsersViewModel
    {
        public Project Project { get; set; }
        public List<PSUser> UsersOnProject { get; set; }
        public List<PSUser>  UsersNotOnProject { get; set; }
    }
}