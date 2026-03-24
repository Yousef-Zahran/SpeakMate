using SpeakMate.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace SpeakMate.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        

        public DbSet<Member> Members { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<MemberLike> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageCorrection> MessageCorrections { get; set; }
        public DbSet<SavedWord> SavedWords { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityRole>().HasData(
             new IdentityRole { Id = "member-id", Name = "Member", NormalizedName = "MEMBER" },
             new IdentityRole { Id = "moderator-id", Name = "Moderator", NormalizedName = "MODERATOR" },
             new IdentityRole { Id = "admin-id", Name = "Admin", NormalizedName = "ADMIN" }
    );

            modelBuilder.Entity<MemberLike>().
                HasKey(x => new { x.SourceMemberId, x.TargetMemberId });

            modelBuilder.Entity<MemberLike>()
                .HasOne(s=>s.SourceMember)
                .WithMany(t=>t.LikedMembers)
                .HasForeignKey(s=>s.SourceMemberId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<MemberLike>()
                .HasOne(s=>s.TargetMember)
                .WithMany(t=>t.LikedByMembers)
                .HasForeignKey(s=>s.TargetMemberId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Message>()
                .HasOne(s=>s.Sender)
                .WithMany(s=>s.MessagesSent)
                .HasForeignKey(s=>s.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(r=>r.Recipient)
                .WithMany(r=>r.MessagesReceived)
                .HasForeignKey(r=>r.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<MessageCorrection>()
                .HasOne(c => c.Message)
                .WithMany(m => m.Corrections)
                .HasForeignKey(c => c.MessageId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<MessageCorrection>()
             .HasOne(c => c.CorrectedBy)
             .WithMany()
             .HasForeignKey(c => c.CorrectedById)
             .IsRequired(true)                  
             .OnDelete(DeleteBehavior.Restrict); 

           
            modelBuilder.Entity<SavedWord>()
                .HasOne(w => w.Member)
                .WithMany(m => m.SavedWords)
                .HasForeignKey(w => w.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

           
            modelBuilder.Entity<SavedWord>()
                .HasOne(w => w.Message)
                .WithMany()
                .HasForeignKey(w => w.MessageId)
                .IsRequired(false)              
                .OnDelete(DeleteBehavior.SetNull);


        }


    }
}
