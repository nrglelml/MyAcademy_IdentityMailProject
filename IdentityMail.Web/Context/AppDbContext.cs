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

        public DbSet<UserMessage> UserMessages { get; set; }
        public DbSet<MessageFolder> MessageFolders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Draft> Drafts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasMany(u => u.SentMessages)
                .WithOne(m => m.Sender)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AppUser>()
                .HasMany(u => u.ReceivedMessages)
                .WithOne(m => m.Receiver)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserMessage>()
                .HasOne(m => m.ParentMessage)
                .WithMany()
                .HasForeignKey(m => m.ParentMessageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserMessage>()
                .HasOne(m => m.Category)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);


            builder.Entity<MessageFolder>()
                .HasOne(mf => mf.Message)
                .WithMany(m => m.MessageFolders)
                .HasForeignKey(mf => mf.MessageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MessageFolder>()
                .HasOne(mf => mf.User)
                .WithMany()
                .HasForeignKey(mf => mf.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MessageFolder>()
                .HasIndex(mf => new { mf.UserId, mf.FolderType, mf.IsDeleted });

            builder.Entity<Report>()
                .HasOne(r => r.Message)
                .WithMany()
                .HasForeignKey(r => r.MessageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Report>()
                .HasOne(r => r.ReportedByUser)
                .WithMany()
                .HasForeignKey(r => r.ReportedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Report>()
                .HasIndex(r => r.Status);

            builder.Entity<Draft>()
                .HasOne(d => d.Sender)
                .WithMany()
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Draft>()
                .HasOne(d => d.Category)
                .WithMany()
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Draft>()
                .HasOne(d => d.ParentMessage)
                .WithMany()
                .HasForeignKey(d => d.ParentMessageId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Draft>()
                .HasIndex(d => d.SenderId);

            builder.Entity<AppUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<AppUser>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            builder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique(); 
        }
    }
}