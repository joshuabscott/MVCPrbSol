using Microsoft.AspNetCore.Mvc.Rendering;
using MVCPrbSol.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCPrbSol.Models.ViewModels
{
    public class ManageProjectUsersViewModel
    {
        public Project Project { get; set; }
        public MultiSelectList Users { get; set; }
        public string[] SelectedUsers { get; set; }

        public MultiSelectList MultiSelectUsersOnProject { get; set; }
        public MultiSelectList MultiSelectUsersOffProject { get; set; }

        public string[] SelectedUsersOnProject { get; set; }
        public string[] SelectedUsersOffProject { get; set; }

        public List<PSUser> UsersOnProject { get; set; }
        public List<PSUser> UsersOffProject { get; set; }
    }
}