using System;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Matchmaking;
using api.Models;
using api.Models.Mentor;
using api.Services;
using api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests
{
    public class MentorServiceTests
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        private MentorService CreateService(AppDbContext ctx)
        {
            IMatchmakingService matchmakingService = new MatchmakingService(ctx);
            return new MentorService(ctx, matchmakingService);
        }

        private static User NewUser(int id, string username) => new User
        {
            Id = id,
            Username = username,
            Password = "Test@123"
        };

        [Fact]
        public async Task GetMentorTeams_ReturnsDistinctDtos_ForEachLobby()
        {
            using var ctx = CreateContext();

            var category = new Category { Name = "Web" };
            var backendRole = new Role { Name = "Backend", Category = category };
            var frontendRole = new Role { Name = "Frontend", Category = category };
            ctx.Categories.Add(category);
            ctx.Roles.AddRange(backendRole, frontendRole);

            var mentorUserId = 100;
            var mentorUser = NewUser(mentorUserId, "mentor_user");
            var mentor = new Mentor { User = mentorUser, UserId = mentorUserId, CreatedAt = DateTime.UtcNow };
            ctx.Users.Add(mentorUser);
            ctx.Mentors.Add(mentor);

            var repUser1 = NewUser(201, "rep1");
            var repUser2 = NewUser(301, "rep2");
            var extraUser1 = NewUser(202, "extra1");
            var extraUser2 = NewUser(302, "extra2");
            ctx.Users.AddRange(repUser1, repUser2, extraUser1, extraUser2);
            await ctx.SaveChangesAsync();

            var repSelection1 = new UserSelection { Id = 1, User = repUser1, Category = category, Role = backendRole };
            var repSelection2 = new UserSelection { Id = 2, User = repUser2, Category = category, Role = frontendRole };
            var extraSelection1 = new UserSelection { Id = 3, User = extraUser1, Category = category, Role = backendRole };
            var extraSelection2 = new UserSelection { Id = 4, User = extraUser2, Category = category, Role = frontendRole };
            ctx.UserSelections.AddRange(repSelection1, repSelection2, extraSelection1, extraSelection2);
            await ctx.SaveChangesAsync();

            var lobby1 = new Lobby { MentorId = mentor.Id, Status = LobbyStatus.Working, CreatedAt = DateTime.UtcNow };
            var lobby2 = new Lobby { MentorId = mentor.Id, Status = LobbyStatus.SchedulingMeeting, CreatedAt = DateTime.UtcNow };
            ctx.Lobbies.AddRange(lobby1, lobby2);
            await ctx.SaveChangesAsync();

            int selId(UserSelection us) => (int)ctx.Entry(us).Property("Id").CurrentValue!;
            ctx.LobbyMembers.AddRange(
                new LobbyMember { UserId = repUser1.Id, LobbyId = lobby1.Id, UserSelectionId = selId(repSelection1), Status = LobbyMember.QueueStatus.FoundLobby },
                new LobbyMember { UserId = extraUser1.Id, LobbyId = lobby1.Id, UserSelectionId = selId(extraSelection1), Status = LobbyMember.QueueStatus.FoundLobby },
                new LobbyMember { UserId = repUser2.Id, LobbyId = lobby2.Id, UserSelectionId = selId(repSelection2), Status = LobbyMember.QueueStatus.FoundLobby },
                new LobbyMember { UserId = extraUser2.Id, LobbyId = lobby2.Id, UserSelectionId = selId(extraSelection2), Status = LobbyMember.QueueStatus.FoundLobby }
            );
            await ctx.SaveChangesAsync();

            var service = CreateService(ctx);

            var result = await service.GetMentorTeams(mentorUserId);

            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
            Assert.Equal(result.Length, result.Distinct().Count());
            var lobbyIds = result.Select(r => r.LobbyId).ToArray();
            Assert.All(lobbyIds, id => Assert.True(id.HasValue));
            Assert.Equal(lobbyIds.Length, lobbyIds.Distinct().Count());
            Assert.Contains(result, r => r.LobbyId == lobby1.Id && r.Status == LobbyStatus.Working);
            Assert.Contains(result, r => r.LobbyId == lobby2.Id && r.Status == LobbyStatus.SchedulingMeeting);
        }

        [Fact]
        public async Task GetMentorTeams_ReturnsEmpty_WhenNoLobbies()
        {
            using var ctx = CreateContext();
            var mentorUserId = 400;
            var mentorUser = NewUser(mentorUserId, "mentorX");
            var mentor = new Mentor { User = mentorUser, UserId = mentorUserId };
            ctx.Users.Add(mentorUser);
            ctx.Mentors.Add(mentor);
            await ctx.SaveChangesAsync();

            var service = CreateService(ctx);
            var result = await service.GetMentorTeams(mentorUserId);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetMentorTeams_Throws_WhenMentorNotFound()
        {
            using var ctx = CreateContext();
            var service = CreateService(ctx);
            var ex = await Assert.ThrowsAsync<Exception>(() => service.GetMentorTeams(999));
            Assert.Equal("Mentor not found for the current user.", ex.Message);
        }
    }
}
