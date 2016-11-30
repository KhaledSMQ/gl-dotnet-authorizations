﻿namespace GeekLearning.Authorizations.EntityFrameworkCore
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class AuthorizationContextExtensions
    {
        public static void AddAuthorizationContext(this ModelBuilder modelBuilder, ModelBuilderOptions options = null)
        {
            var schemaName = options?.SchemaName;
            if (string.IsNullOrEmpty(schemaName))
            {
                // Force null value for empty values
                schemaName = null;
            }

            modelBuilder.Entity<Scope>(entity =>
            {
                entity.MapToTable("Scope", schemaName);
                entity.HasIndex(s => s.Name).IsUnique();
            });

            modelBuilder.Entity<ScopeHierarchy>(entity =>
            {
                entity.MapToTable("ScopeHierarchy", schemaName);
                entity.HasKey(sh => new { sh.ParentId, sh.ChildId });
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.MapToTable("Role", schemaName);
                entity.HasIndex(r => r.Name).IsUnique();
            });

            modelBuilder.Entity<Right>(entity =>
            {
                entity.MapToTable("Right", schemaName);
                entity.HasIndex(r => r.Name).IsUnique();
            });

            modelBuilder.Entity<RoleRight>(entity =>
            {
                entity.MapToTable("RoleRight", schemaName);
                entity.HasKey(rr => new { rr.RoleId, rr.RightId });
            });

            modelBuilder.Entity<Principal>(entity => entity.MapToTable("Principal", schemaName));

            modelBuilder.Entity<Authorization>(entity =>
            {
                entity.MapToTable("Authorization", schemaName);
                entity.HasIndex(a => new { a.RoleId, a.ScopeId, a.PrincipalId }).IsUnique();
            });
        }

        public static DbSet<Authorization> Authorizations<TContext>(this TContext context)
            where TContext : DbContext
        {
            return context.Set<Authorization>();
        }

        public static DbSet<Principal> Principals<TContext>(this TContext context)
            where TContext : DbContext
        {
            return context.Set<Principal>();
        }

        public static DbSet<Right> Rights<TContext>(this TContext context)
            where TContext : DbContext
        {
            return context.Set<Right>();
        }

        public static DbSet<Role> Roles<TContext>(this TContext context)
            where TContext : DbContext
        {
            return context.Set<Role>();
        }

        public static DbSet<RoleRight> RoleRights<TContext>(this TContext context)
            where TContext : DbContext
        {
            return context.Set<RoleRight>();
        }

        public static DbSet<Scope> Scopes<TContext>(this TContext context)
           where TContext : DbContext
        {
            return context.Set<Scope>();
        }

        public static DbSet<ScopeHierarchy> ScopeHierarchies<TContext>(this TContext context)
           where TContext : DbContext
        {
            return context.Set<ScopeHierarchy>();
        }

        public static PropertyBuilder<TProperty> AddPrincipalRelationship<TProperty>(this PropertyBuilder<TProperty> propertyBuilder)
        {
            return propertyBuilder.HasAnnotation("ForeignKey", "Principal");
        }

        private static EntityTypeBuilder<TEntity> MapToTable<TEntity>(this EntityTypeBuilder<TEntity> builder, string tableName, string schema = null)
            where TEntity : class
        {
            if (schema == null)
            {
                builder.ToTable(tableName);
            }
            else
            {
                builder.ToTable(tableName, schema);
            }

            return builder;
        }
    }
}
