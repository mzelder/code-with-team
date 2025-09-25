using api.Controllers;
using api.Data;
using api.Dtos.Auth;
using api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace api.Tests
{
    public class AuthControllerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            context.Users.Add(new Models.User { Username = "testuser", Password = "1234" });
            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ReturnsUnauthorized()
        {
            var context = GetDbContext();
            var controller = new AuthController(context);
            var dto = new LoginDto { Username = "testuser", Password = "wrongpass" };

            var result = await controller.Login(dto);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Register_UserDoesntExistInDb_ReturnsOk()
        {
            var context = GetDbContext();
            var controller = new AuthController(context);
            var dto = new RegisterDto { Username = "newuser", Password = "1234", ConfirmPassword = "1234" };

            var result = await controller.Register(dto);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Register_UserExistInDb_ReturnsBadRequest()
        {
            var context = GetDbContext();
            var controller = new AuthController(context);
            var dto = new RegisterDto { Username = "testuser", Password = "1234", ConfirmPassword = "1234" };

            var result = await controller.Register(dto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Register_PasswordsNotEqual_ReturnsBadRequest()
        {
            var context = GetDbContext();
            var controller = new AuthController(context);
            var dto = new RegisterDto { Username = "newuser", Password = "goodpass", ConfirmPassword = "badpass" };


            var result = await controller.Register(dto);

            Assert.IsType<BadRequestObjectResult>(result);
        }
        
        [Fact]
        public async Task Register_PasswordsEqual_ReturnsOk()
        {
            var context = GetDbContext();
            var controller = new AuthController(context);
            var dto = new RegisterDto { Username = "newuser", Password = "goodpass", ConfirmPassword = "goodpass" };


            var result = await controller.Register(dto);

            Assert.IsType<OkObjectResult>(result);
        }

    }
}