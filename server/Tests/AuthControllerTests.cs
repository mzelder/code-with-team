using api.Data;
using api.Models;
using api.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace api.Tests
{
    public class AuthControllerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var context = new AppDbContext(options);
            context.Users.Add(new Models.User { Username = "testuser", Password = "1234" });
            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsOk()
        {
            var context = GetDbContext();
            var controller = new AuthController(context);
            var user = new User { Username = "testuser", Password = "1234" };

            var result = await controller.Login(user);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ReturnsUnauthorized()
        {
            var context = GetDbContext();
            var controller = new AuthController(context);
            var user = new User { Username = "testuser", Password = "wrongpass" };

            var result = await controller.Login(user);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Register_UserDoesntExistInDb_ReturnsOk()
        {
            var context = GetDbContext();
            var controller = new AuthController(context);
            var user = new User { Username = "newuser", Password = "1234" };

            var result = await controller.Register(user);

            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async Task Register_UserExistInDb_ReturnsOk()
        {
            var context = GetDbContext();
            var controller = new AuthController(context);
            var user = new User { Username = "testuser", Password = "1234" };

            var result = await controller.Register(user);

            Assert.IsType<BadRequestObjectResult>(result);
        }

    }
}