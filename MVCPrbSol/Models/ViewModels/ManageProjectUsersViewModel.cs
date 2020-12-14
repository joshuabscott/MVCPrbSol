using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCPrbSol.Models.ViewModels
{
    public class ManageProjectUsersViewModel
    {
        public Project Project { get; set; }
        public MultiSelectList Users { get; set; }
        public string[] SelectedUsers { get; set; }

        public PSUser User { get; set; }
        public IEnumerable<string> UserRole { get; set; }
        public ICollection<Project> CurrentProjects { get; set; }
        public SelectList Projects { get; set; }
        public int SelectedProject { get; set; }
    }
}// Manage what Users can be on what project