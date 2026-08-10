using Microsoft.EntityFrameworkCore;
using MediFormManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Infrastructure.Persistence
{
    public class MediFormDbContext : DbContext
    {
        public MediFormDbContext(DbContextOptions<MediFormDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<JobPosition> JobPositions => Set<JobPosition>();
        public DbSet<FormCategory> FormCategories => Set<FormCategory>();
        public DbSet<Form> Forms => Set<Form>();
        public DbSet<FormVersion> FormVersions => Set<FormVersion>();
        public DbSet<FormComponent> FormComponents => Set<FormComponent>();
        public DbSet<MedicalOrder> Orders => Set<MedicalOrder>();
        public DbSet<OrderFormMapping> OrderMappings => Set<OrderFormMapping>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure entity relationships and constraints here if needed
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MediFormDbContext).Assembly);
        }
    }
}


