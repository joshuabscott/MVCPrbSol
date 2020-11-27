using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCPrbSol.Models;

namespace MVCPrbSol.Data
{
    public class ApplicationDbContext : IdentityDbContext<PSUser>//this is the class that connects directly to the Database
    {
        //ApplicatioDbContext is a representation of our database by links our classes to our Database entities
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)//allows us to connect to default database using appsettins.json
        {//This is were our Authorization and Authentication comes from
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProjectUser>()
                .HasKey(pu => new { pu.ProjectId, pu.UserId });
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }

        public DbSet<TicketAttachment> TicketAttachments { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }
        public DbSet<TicketHistory> TicketHistories { get; set; }
        public DbSet<TicketPriority> TicketPriorities { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }

        //public DbSet<PSUser> PSUsers { get; set; }
        public DbSet<Notification> Notifications { get; set; }

    }
}
//SAT