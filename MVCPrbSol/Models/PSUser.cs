using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MVCPrbSol.Models
{
    public class PSUser : IdentityUser
    {
        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        [NotMapped]
        public string FullName { get { return $"{FirstName} {LastName}"; } }

        [Display (Name = "Avatar")]
        public string ImagePath { get; set; }
        public byte[] ImageData { get; set; }

        public List<ProjectUser> ProjectUsers { get; set; }

    }
}//Data of User to be used with IdentityUser
//Sat
//Mon