using api.Models;
using api.Models.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<UserLanguage> UserLanguages { get; set; }
        public DbSet<UserSelection> UserSelections { get; set; }
        public DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }
        public DbSet<LobbyMember> LobbyMembers { get; set; }
        public DbSet<Lobby> Lobbies { get; set; }
        public DbSet<TaskDefinitions> TaskDefinitions { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<UserTaskProgress> UserTaskProgresses { get; set; }
        public DbSet<TeamTask> TeamTasks { get; set; }
        public DbSet<TeamTaskProgress> TeamTaskProgresses { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<MeetingProposal> MeetingProposals { get; set; }
        public DbSet<MeetingVote> MeetingVotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Category ↔ Roles
            modelBuilder.Entity<Role>()
                .HasOne(r => r.Category)
                .WithMany(c => c.Roles)
                .HasForeignKey(r => r.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            // Category ↔ UserSelections
            modelBuilder.Entity<UserSelection>()
                .HasOne(us => us.Category)
                .WithMany(c => c.UserSelections)
                .HasForeignKey(us => us.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            // Role ↔ ProgrammingLanguages
            modelBuilder.Entity<ProgrammingLanguage>()
                .HasOne(pl => pl.Role)
                .WithMany(r => r.ProgrammingLanguages)
                .HasForeignKey(pl => pl.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            // Role ↔ UserSelections
            modelBuilder.Entity<UserSelection>()
                .HasOne(us => us.Role)
                .WithMany(r => r.UserSelections)
                .HasForeignKey(us => us.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            // User ↔ UserSelections
            modelBuilder.Entity<UserSelection>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserSelections)
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.NoAction); // prevent multiple cascade paths

            // User ↔ LobbyMembers
            modelBuilder.Entity<LobbyMember>()
                .HasOne(lq => lq.User)
                .WithMany(u => u.LobbbyQueues)
                .HasForeignKey(lq => lq.UserId)
                .OnDelete(DeleteBehavior.NoAction); // prevent multiple cascade paths

            // UserSelection ↔ UserLanguages
            modelBuilder.Entity<UserLanguage>()
                .HasOne(ul => ul.UserSelection)
                .WithMany(us => us.UserLanguages)
                .HasForeignKey(ul => ul.UserSelectionId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<UserSelection>()
                .HasOne(us => us.LobbyQueue)
                .WithOne(lq => lq.UserSelection)
                .HasForeignKey<UserSelection>(us => us.Id)
                .OnDelete(DeleteBehavior.Cascade);

            // ProgrammingLanguage ↔ UserLanguages
            modelBuilder.Entity<UserLanguage>()
                .HasOne(ul => ul.ProgrammingLanguage)
                .WithMany(pl => pl.UserLanguages)
                .HasForeignKey(ul => ul.ProgrammingLanguageId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserTaskProgress>()
                .HasOne(utp => utp.LobbyMember)
                .WithOne(lm => lm.UserTaskProgress)
                .HasForeignKey<UserTaskProgress>(utp => utp.LobbyMemberId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeamTaskProgress>()
                .HasOne(ttp => ttp.Lobby)
                .WithOne(l => l.TeamTaskProgress)
                .HasForeignKey<TeamTaskProgress>(ttp => ttp.LobbyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MeetingVote>()
                .HasOne(mv => mv.User)
                .WithMany()
                .HasForeignKey(mv => mv.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MeetingVote>()
                .HasOne(mv => mv.MeetingProposal)
                .WithMany(mp => mp.Votes)
                .HasForeignKey(mv => mv.MeetingProposalId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<LobbyMember>()
                .Property(u => u.JoinedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Web Dev" }
            );

            modelBuilder.Entity<Role>().HasData(
               new Role { Id = 1, Name = "Frontend", CategoryId = 1 },
               new Role { Id = 2, Name = "Backend", CategoryId = 1 },
               new Role { Id = 3, Name = "QA", CategoryId = 1 },
               new Role { Id = 4, Name = "PM", CategoryId = 1 }
           );

            modelBuilder.Entity<ProgrammingLanguage>().HasData(
                // Frontend
                new ProgrammingLanguage { Id = 1, Name = "JavaScript", RoleId = 1 },

                // Backend
                new ProgrammingLanguage { Id = 2, Name = "Python", RoleId = 2 },

                // QA - same as backend
                new ProgrammingLanguage { Id = 3, Name = "Python", RoleId = 3 },

                // PM
                new ProgrammingLanguage { Id = 4, Name = "Standard", RoleId = 4 }
            );

            modelBuilder.Entity<TaskDefinitions>().HasData(
                // Team Tasks
                new TaskDefinitions
                {
                    Id = 1,
                    Name = "Book meeting",
                    Description = "",
                    Category = TaskCategory.Team
                },
                new TaskDefinitions
                {
                    Id = 2,
                    Name = "Attend your scheduled team meeting",
                    Description = "",
                    Category = TaskCategory.Team
                },
                new TaskDefinitions
                {
                    Id = 3,
                    Name = "Break down the tasks",
                    Description = "",
                    Category = TaskCategory.Team
                },

                // User Tasks
                new TaskDefinitions
                {
                    Id = 4,
                    Name = "Visit github repository",
                    Description = "",
                    Category = TaskCategory.User
                },
                new TaskDefinitions
                {
                    Id = 5,
                    Name = "Start coding",
                    Description = "",
                    Category = TaskCategory.User
                }
            );
        }
    }
}
