using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCPrbSol.Models.ViewModels
{
    public class ManageUserRolesViewModel
    {
        public PSUser User { get; set; }
        public MultiSelectList Roles { get; set; }
        public string[] SelectedRoles { get; set; }
    }
}//Manage the Roles of the Users