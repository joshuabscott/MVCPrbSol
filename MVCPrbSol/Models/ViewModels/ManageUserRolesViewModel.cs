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

        public string UserId { get; set; }
        public string UserName { get; set; }
        //public SelectList Roles { get; set; }
        public IEnumerable<string> UserRole { get; set; }
        public string SelectedRole { get; set; }
    }
}//Manage the Roles of the Users