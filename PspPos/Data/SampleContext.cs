﻿using PspPos.Models;
using Microsoft.EntityFrameworkCore;
using PspPos.Models.User;

namespace PspPos.Data
{
    public class SampleContext : DbContext
    {
        public SampleContext(DbContextOptions<SampleContext> options) : base(options)
        {

        }

        public DbSet<Sample> Samples => Set<Sample>();
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sample>().ToTable("Samples");
            modelBuilder.Entity<Company>().ToTable("Companies");

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>()
                .HasOne(u => u.Company)
                .WithMany()
                .HasForeignKey(u => u.CompanyId);
        }
    }
}


    