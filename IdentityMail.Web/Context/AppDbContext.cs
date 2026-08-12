using IdentityMail.Web.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityMail.Web.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {



        public AppDbContext(DbContextOptions options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<AppUser>().HasMany(message => message.SentMessages)
                                     .WithOne(s => s.Sender).HasForeignKey(x => x.SenderId)
                                     .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AppUser>().HasMany(message => message.ReceivedMessages)
                                     .WithOne(s => s.Receiver).HasForeignKey(x => x.ReceiverId)
                                     .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(builder);
            builder.Entity<AppUser>()
            .HasIndex(u => u.Email)
            .IsUnique();

            builder.Entity<AppUser>()
                .HasIndex(u => u.UserName)
                .IsUnique();
        }


        public DbSet<UserMessage> UserMessages { get; set; }



    }
}