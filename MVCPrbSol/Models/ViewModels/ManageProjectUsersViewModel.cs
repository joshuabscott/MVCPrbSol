using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCPrbSol.Models.ViewModels
{
    public class ManageProjectUsersViewModel
    {
        public Project Project { get; set; }

        public MultiSelectList Users { get; set; }  //Populates list box???

        public string[] SelectedProjects { get; set; } //receives selected users????
    }
}