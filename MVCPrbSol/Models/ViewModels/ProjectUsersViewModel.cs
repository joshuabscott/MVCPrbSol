using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCPrbSol.Models.ViewModels
{
    public class ProjectUsersViewModel
    {
        public Project Project { get; set; }
        
        public MultiSelectList UsersOnProject { get; set; }
        public MultiSelectList UsersOffProject { get; set; }
        
        public string[] SelectedUsers { get; set; }
    }
}
