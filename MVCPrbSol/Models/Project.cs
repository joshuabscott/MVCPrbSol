﻿using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCPrbSol.Models
{
    public class Project
        //prob tab tab gives this code
    {
        public Project ()
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
}
