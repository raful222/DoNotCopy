using DoNotCopy.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DoNotCopy.Core.Entities.Identity;
using DoNotCopy.Core.Entities;

namespace DoNotCopy.Core.Data
{
    /*
     * Add-Migration -OutputDir "Data/Migrations" InitialSchema -Project DoNotCopy.Core -StartupProject DoNotCopy
     * Remove-Migration -Force -Project DoNotCopy.Core -StartupProject DoNotCopy
     * Update-Database -Project DoNotCopy.Core -StartupProject DoNotCopy
     */
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int,
        ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
        ApplicationRoleClaim, ApplicationUserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.EnableSensitiveDataLogging();
        //}

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AddTimestamps()
        {
            var now = DateTime.Now;

            foreach (var auditableEntity in ChangeTracker.Entries<IAuditable>())
            {
                if (auditableEntity.State == EntityState.Added)
                {
                    auditableEntity.Entity.CreatedAt = now;
                    auditableEntity.Entity.UpdatedAt = now;
                }

                if (auditableEntity.State == EntityState.Modified)
                {
                    auditableEntity.Property(nameof(IAuditable.CreatedAt)).IsModified = false;
                    auditableEntity.Entity.UpdatedAt = now;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(b =>
            {
                b.ToTable(name: "Users");



                b.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                b.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                b.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<ApplicationRole>(b =>
            {
                b.ToTable(name: "Roles");

                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();
            });

            builder.Entity<ApplicationUserRole>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            builder.Entity<ApplicationUserClaim>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<ApplicationUserLogin>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<ApplicationRoleClaim>(entity =>
            {
                entity.ToTable("RoleClaims");
            });

            builder.Entity<ApplicationUserToken>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            builder.Entity<File>(entity =>
               entity.HasOne(x => x.ExerciseFile)
                   .WithOne(x => x.File)
                   .HasForeignKey<ExerciseFile>(x => x.Id)
                   .OnDelete(DeleteBehavior.Cascade));

            builder.Entity<Exercise>(entity =>
                entity.HasMany(x => x.ImageFiles)
                    .WithOne(x => x.Exercise)
                    .HasForeignKey(x => x.ExerciseId)
                    .OnDelete(DeleteBehavior.Cascade));

            // Seeding initial data
            builder.Entity<ApplicationRole>().HasData(new ApplicationRole
            {
                Id = 1,
                Name = "Administrator",
                NormalizedName = "Administrator".ToUpper()
            });

            var user = new ApplicationUser
            {
                Id = 1,
                Email = "rafi@gmail.co.il",
                NormalizedEmail = "rafi@gmail.co.il".ToUpper(),
                EmailConfirmed = true,
                PhoneNumber = null,
                FirstName = "בדיקה",
                LastName = "רפאל",
                UserName = "rafi@gmail.co.il",
                NormalizedUserName = "rafi@gmail.co.il".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString("D"),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, "154154");

            builder.Entity<ApplicationUser>().HasData(user);

            builder.Entity<ApplicationUserRole>().HasData(new ApplicationUserRole
            {
                RoleId = 1,
                UserId = 1
            });



        }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<StudentWithCourse> StudentWithCourses { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentFile> StudentFiles { get; set; }
        public virtual DbSet<Lecturer> Lecturers { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<Exercise> Exercises { get; set; }
        public virtual DbSet<ExerciseFile> ExerciseFiles { get; set; }

    }
}
