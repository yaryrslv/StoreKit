using StoreKit.Application.Abstractions.Services.General;
using StoreKit.Application.Abstractions.Services.Identity;
using StoreKit.Domain.Contracts;
using StoreKit.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StoreKit.Infrastructure.Persistence
{
    public class ApplicationDbContext : BaseDbContext
    {
        private readonly ISerializerService _serializer;
        public IDbConnection Connection => Database.GetDbConnection();
        private readonly ICurrentUser _currentUserService;
        private readonly ITenantService _tenantService;
        public ApplicationDbContext(DbContextOptions options, ITenantService tenantService, ICurrentUser currentUserService, ISerializerService serializer)
        : base(options, tenantService, currentUserService, serializer)
        {
            _tenantService = tenantService;
            _currentUserService = currentUserService;
            _serializer = serializer;
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(p => p.Product)
                .WithOne(p => p.Category)
                .IsRequired();
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var currentUserId = _currentUserService.GetUserId();
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = currentUserId;
                        entry.Entity.LastModifiedBy = currentUserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = currentUserId;
                        break;
                    case EntityState.Deleted:
                        if (entry.Entity is ISoftDelete softDelete)
                        {
                            softDelete.DeletedBy = currentUserId;
                            softDelete.DeletedOn = DateTime.UtcNow;
                            entry.State = EntityState.Modified;
                        }

                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}