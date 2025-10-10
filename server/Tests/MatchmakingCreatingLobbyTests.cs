using api.Data;
using api.Models;
using api.Services;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    public class MatchmakingCreatingLobbyTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetLobbyStatusAsync_WithEnoughUniqueRoles_ReturnsLobby()
        {
            var db = GetDbContext();

            var category = new Category { Id = 1, Name = "Web Development" };
            db.Categories.Add(category);

            var roles = new List<Role>
            {
                new Role { Id = 1, Name = "Role1", CategoryId = 1, Category = category },
                new Role { Id = 2, Name = "Role2", CategoryId = 1, Category = category },
                new Role { Id = 3, Name = "Role3", CategoryId = 1, Category = category },
                new Role { Id = 4, Name = "Role4", CategoryId = 1, Category = category }
            };
            db.Roles.AddRange(roles);

            var users = new List<User>
            {
                new User { Id = 1, Username = "User1", Password = "pass" },
                new User { Id = 2, Username = "User2", Password = "pass" },
                new User { Id = 3, Username = "User3", Password = "pass" },
                new User { Id = 4, Username = "User4", Password = "pass" }
            };
            db.Users.AddRange(users);

            for (int i = 0; i < 4; i++)
            {
                var selection = new UserSelection
                {
                    Id = i + 1,
                    UserId = users[i].Id,
                    User = users[i],
                    CategoryId = category.Id,
                    Category = category,
                    RoleId = roles[i].Id,
                    Role = roles[i]
                };
                db.UserSelections.Add(selection);

                db.LobbyMembers.Add(new LobbyMember
                {
                    Id = i + 1,
                    UserId = users[i].Id,
                    User = users[i],
                    UserSelectionId = selection.Id,
                    UserSelection = selection,
                    Status = LobbyMember.QueueStatus.InQueue,
                    JoinedAt = DateTime.UtcNow
                });
            }

            db.SaveChanges();
            var service = new MatchmakingService(db);

            await service.FormLobbiesAsync();
            var result = await service.GetLobbyStatusAsync(users[0].Id);

            Assert.True(result.Found);
            Assert.NotNull(result.LobbyId);
            Assert.Equal(4, result.Members!.Count);
        }

        [Fact]
        public async Task GetLobbyStatusAsync_NotEnoughUsers_ReturnsNull()
        {
            var db = GetDbContext();

            var category = new Category { Id = 1, Name = "Web Development" };
            db.Categories.Add(category);

            var roles = new List<Role>
            {
                new Role { Id = 1, Name = "Role1", CategoryId = 1, Category = category },
                new Role { Id = 2, Name = "Role2", CategoryId = 1, Category = category },
                new Role { Id = 3, Name = "Role3", CategoryId = 1, Category = category },
                new Role { Id = 4, Name = "Role4", CategoryId = 1, Category = category }
            };
            db.Roles.AddRange(roles);

            var users = new List<User>
            {
                new User { Id = 1, Username = "User1", Password = "pass" },
                new User { Id = 2, Username = "User2", Password = "pass" },
                new User { Id = 3, Username = "User3", Password = "pass" }
            };
            db.Users.AddRange(users);

            for (int i = 0; i < users.Count; i++)
            {
                var selection = new UserSelection
                {
                    Id = i + 1,
                    UserId = users[i].Id,
                    User = users[i],
                    CategoryId = category.Id,
                    Category = category,
                    RoleId = roles[i].Id,
                    Role = roles[i]
                };
                db.UserSelections.Add(selection);

                db.LobbyMembers.Add(new LobbyMember
                {
                    Id = i + 1,
                    UserId = users[i].Id,
                    User = users[i],
                    UserSelectionId = selection.Id,
                    UserSelection = selection,
                    Status = LobbyMember.QueueStatus.InQueue,
                    JoinedAt = DateTime.UtcNow
                });
            }

            db.SaveChanges();
            var service = new MatchmakingService(db);

            await service.FormLobbiesAsync();
            var result = await service.GetLobbyStatusAsync(users[0].Id);

            Assert.False(result.Found);
            Assert.Null(result.LobbyId);
            Assert.Null(result.Members);
        }

        [Fact]
        public async Task GetLobbyStatusAsync_WithoutEnoughUniqueRoles_ReturnsNull()
        {
            var db = GetDbContext();

            var category = new Category { Id = 1, Name = "Web Development" };
            db.Categories.Add(category);

            var roles = new List<Role>
            {
                new Role { Id = 1, Name = "Role1", CategoryId = 1, Category = category },
                new Role { Id = 2, Name = "Role2", CategoryId = 1, Category = category },
                new Role { Id = 3, Name = "Role3", CategoryId = 1, Category = category },
                new Role { Id = 4, Name = "Role4", CategoryId = 1, Category = category }
            };
            db.Roles.AddRange(roles);

            var users = new List<User>
            {
                new User { Id = 1, Username = "User1", Password = "pass" },
                new User { Id = 2, Username = "User2", Password = "pass" },
                new User { Id = 3, Username = "User3", Password = "pass" },
                new User { Id = 4, Username = "User4", Password = "pass" },
            };
            db.Users.AddRange(users);

            for (int i = 0; i < users.Count; i++)
            {
                var selection = new UserSelection
                {
                    Id = i + 1,
                    UserId = users[i].Id,
                    User = users[i],
                    CategoryId = category.Id,
                    Category = category,
                    RoleId = roles[0].Id,
                    Role = roles[0]
                };
                db.UserSelections.Add(selection);

                db.LobbyMembers.Add(new LobbyMember
                {
                    Id = i + 1,
                    UserId = users[i].Id,
                    User = users[i],
                    UserSelectionId = selection.Id,
                    UserSelection = selection,
                    Status = LobbyMember.QueueStatus.InQueue,
                    JoinedAt = DateTime.UtcNow
                });
            }

            db.SaveChanges();
            var service = new MatchmakingService(db);

            await service.FormLobbiesAsync();
            var result = await service.GetLobbyStatusAsync(users[0].Id);

            Assert.False(result.Found);
            Assert.Null(result.LobbyId);
            Assert.Null(result.Members);
        }

        [Fact]
        public async Task GetLobbyStatusAsync_MoreThanEnoguhUsersWithUniqueRoles_ReturnsLobby()
        {
            var db = GetDbContext();

            var category = new Category { Id = 1, Name = "Web Development" };
            db.Categories.Add(category);

            var roles = new List<Role>
            {
                new Role { Id = 1, Name = "Role1", CategoryId = 1, Category = category },
                new Role { Id = 2, Name = "Role2", CategoryId = 1, Category = category },
                new Role { Id = 3, Name = "Role3", CategoryId = 1, Category = category },
                new Role { Id = 4, Name = "Role4", CategoryId = 1, Category = category }
            };
            db.Roles.AddRange(roles);

            var users = new List<User>
            {
                new User { Id = 1, Username = "User1", Password = "pass" },
                new User { Id = 2, Username = "User2", Password = "pass" },
                new User { Id = 3, Username = "User3", Password = "pass" },
                new User { Id = 4, Username = "User4", Password = "pass" },
                new User { Id = 5, Username = "User5", Password = "pass" },
                new User { Id = 6, Username = "User6", Password = "pass" }
            };
            db.Users.AddRange(users);
            
            for (int i = 0; i < users.Count; i++)
            {
                var selection = new UserSelection
                {
                    Id = i + 1,
                    UserId = users[i].Id,
                    User = users[i],
                    CategoryId = category.Id,
                    Category = category,
                    RoleId = roles[i % roles.Count].Id,
                    Role = roles[i % roles.Count]
                };
                db.UserSelections.Add(selection);
                db.LobbyMembers.Add(new LobbyMember
                {
                    Id = i + 1,
                    UserId = users[i].Id,
                    User = users[i],
                    UserSelectionId = selection.Id,
                    UserSelection = selection,
                    Status = LobbyMember.QueueStatus.InQueue,
                    JoinedAt = DateTime.UtcNow
                });
            }
            db.SaveChanges();

            var service = new MatchmakingService(db);
            await service.FormLobbiesAsync();
            var memberCount = await db.LobbyMembers.CountAsync();

            var result = await service.GetLobbyStatusAsync(users[0].Id);

            Assert.True(result.Found);
            Assert.NotNull(result.LobbyId);
            Assert.Equal(4, result.Members!.Count);
            Assert.All(result.Members, m => Assert.Contains(m.Name, users.Select(u => u.Username)));
        }

        [Fact]
        public async Task GetLobbyStatusAsync_MoreThanOneCategory_ReturnsMultipleLobbies()
        {
            var db = GetDbContext();

            var category1 = new Category { Id = 1, Name = "Web Development" };
            var category2 = new Category { Id = 2, Name = "Mobile Development" };
            db.Categories.AddRange(category1, category2);

            var rolesCat1 = new List<Role>
            {
                new Role { Id = 1, Name = "Role1", CategoryId = 1, Category = category1 },
                new Role { Id = 2, Name = "Role2", CategoryId = 1, Category = category1 },
                new Role { Id = 3, Name = "Role3", CategoryId = 1, Category = category1 },
                new Role { Id = 4, Name = "Role4", CategoryId = 1, Category = category1 }
            };
            var rolesCat2 = new List<Role>
            {
                new Role { Id = 5, Name = "RoleA", CategoryId = 2, Category = category2 },
                new Role { Id = 6, Name = "RoleB", CategoryId = 2, Category = category2 },
                new Role { Id = 7, Name = "RoleC", CategoryId = 2, Category = category2 },
                new Role { Id = 8, Name = "RoleD", CategoryId = 2, Category = category2 }
            };
            db.Roles.AddRange(rolesCat1);
            db.Roles.AddRange(rolesCat2);

            var usersCat1 = new List<User>
            {
                new User { Id = 1, Username = "User1", Password = "pass" },
                new User { Id = 2, Username = "User2", Password = "pass" },
                new User { Id = 3, Username = "User3", Password = "pass" },
                new User { Id = 4, Username = "User4", Password = "pass" }
            };
            var usersCat2 = new List<User>
            {
                new User { Id = 5, Username = "UserA", Password = "pass" },
                new User { Id = 6, Username = "UserB", Password = "pass" },
                new User { Id = 7, Username = "UserC", Password = "pass" },
                new User { Id = 8, Username = "UserD", Password = "pass" }
            };
            db.Users.AddRange(usersCat1);
            db.Users.AddRange(usersCat2);

            for (int i = 0; i < 4; i++)
            {
                var selection = new UserSelection
                {
                    Id = i + 1,
                    UserId = usersCat1[i].Id,
                    User = usersCat1[i],
                    CategoryId = category1.Id,
                    Category = category1,
                    RoleId = rolesCat1[i].Id,
                    Role = rolesCat1[i]
                };
                db.UserSelections.Add(selection);

                db.LobbyMembers.Add(new LobbyMember
                {
                    Id = i + 1,
                    UserId = usersCat1[i].Id,
                    User = usersCat1[i],
                    UserSelectionId = selection.Id,
                    UserSelection = selection,
                    Status = LobbyMember.QueueStatus.InQueue,
                    JoinedAt = DateTime.UtcNow
                });
            }

            for (int i = 0; i < 4; i++)
            {
                var selection = new UserSelection
                {
                    Id = 5 + i,
                    UserId = usersCat2[i].Id,
                    User = usersCat2[i],
                    CategoryId = category2.Id,
                    Category = category2,
                    RoleId = rolesCat2[i].Id,
                    Role = rolesCat2[i]
                };
                db.UserSelections.Add(selection);

                db.LobbyMembers.Add(new LobbyMember
                {
                    Id = 5 + i,
                    UserId = usersCat2[i].Id,
                    User = usersCat2[i],
                    UserSelectionId = selection.Id,
                    UserSelection = selection,
                    Status = LobbyMember.QueueStatus.InQueue,
                    JoinedAt = DateTime.UtcNow
                });
            }

            db.SaveChanges();
            var service = new MatchmakingService(db);

            // First formation (category1)
            await service.FormLobbiesAsync();
            var statusCat1 = await service.GetLobbyStatusAsync(usersCat1[0].Id);
            Assert.True(statusCat1.Found);
            Assert.Equal(4, statusCat1.Members!.Count);

            // Second formation (remaining category2)
            await service.FormLobbiesAsync();
            var statusCat2 = await service.GetLobbyStatusAsync(usersCat2[0].Id);
            Assert.True(statusCat2.Found);
            Assert.Equal(4, statusCat2.Members!.Count);
        }

        // 6. Empty queue -> Found false
        [Fact]
        public async Task GetLobbyStatusAsync_EmptyQueue_ReturnsNull()
        {
            var db = GetDbContext();
            var service = new MatchmakingService(db);

            await service.FormLobbiesAsync();
            var result = await service.GetLobbyStatusAsync(1);

            Assert.False(result.Found);
            Assert.Null(result.LobbyId);
            Assert.Null(result.Members);
            Assert.Equal(0, await db.LobbyMembers.CountAsync());
            Assert.Equal(0, await db.Lobbies.CountAsync());
            Assert.Equal(0, await db.LobbyMembers.CountAsync());
        }
    }
}
