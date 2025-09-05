using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit.Sdk;

namespace api.Tests
{
    public class AuthIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public AuthIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }
        
        // in the future class need to be rewritten for the multi-use.
        // ex. we want to check if Authorization works on /api/dashboard, /api/profile
        [Fact]
        public async Task Login_ShouldSetCookie_AndAuthorizeMe()
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });

            var loginData = new
            {
                Username = "test",
                Password = "test"
            };

            var loginResponse = await client.PostAsJsonAsync("/api/Auth/login", loginData);
            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

            var setCookie = loginResponse.Headers.GetValues("Set-Cookie");
            Assert.NotNull(setCookie);

            client.DefaultRequestHeaders.Add("Cookie", setCookie);

            var meResponse = await client.GetAsync("/api/Auth/me");
            Assert.Equal(HttpStatusCode.OK, meResponse.StatusCode);

            var meContent = await meResponse.Content.ReadAsStringAsync();
            Assert.Contains("test", meContent);
        }
    }
}
