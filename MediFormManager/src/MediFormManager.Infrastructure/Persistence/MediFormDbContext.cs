using MediFormManager.Domain.Entities.Components;
using MediFormManager.Domain.Entities.Departments;
using MediFormManager.Domain.Entities.Forms;
using MediFormManager.Domain.Entities.Orders;
using MediFormManager.Domain.Entities.Rules;
using MediFormManager.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;

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

        public DbSet<ComponentOption> ComponentOptions => Set<ComponentOption>();
        public DbSet<ComponentRule> ComponentRules => Set<ComponentRule>();
        public DbSet<RuleCondition> RuleConditions => Set<RuleCondition>();
        public DbSet<RuleAction> RuleActions => Set<RuleAction>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure entity relationships and constraints here if needed
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MediFormDbContext).Assembly);
        }
    }
}


