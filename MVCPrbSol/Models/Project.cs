using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace MVCPrbSol.Models   //Namespace is the outermost , Inside is a class, than a method, than the logic
{
    public class Project
    {
        public Project()
        {
            Tickets = new HashSet<Ticket>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Project Name")]
        public string Name { get; set; }

     
        [Display(Name = "Project Image")]
        public string ImagePath { get; set; }
        public byte [] ImageData { get; set; }

        public List<ProjectUser> ProjectUsers { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }

    }
}//Data to be used when creating an Instance of a Project Object of this class
//Updated 11/15/2020
