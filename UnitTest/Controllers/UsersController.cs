using Chat.API.Models;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using UnitTest.Configuration;
using Xunit;
using FluentAssertions;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using AutoFixture;
using System.Net.Http.Headers;
using System.Net.Http;

namespace UnitTest.Controllers
{
    [ExcludeFromCodeCoverage]
    public class UsersController : IClassFixture<ServerStartup>
    {
        public string ServiceUrl = "https://localhost";

        readonly HttpClient _client;

        public UsersController(ServerStartup application)
        {
            _client = application.CreateClient();
        }

        [Fact]
        public async Task Get_Users_ShouldReturnUnauthorized() {
            var urlToCall = new Uri($"{ServiceUrl}/api/v1/users");

            var result = await _client.GetAsync(urlToCall);

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Get_UserById_ShouldReturnUnauthorized() {
            var urlToCall = new Uri($"{ServiceUrl}/api/v1/users/1");

            var result = await _client.GetAsync(urlToCall);

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Put_UserById_ShouldReturnUnauthorized() {
            var urlToCall = new Uri($"{ServiceUrl}/api/v1/users/1");

            var payload = new User() {
                Name = "guigui",
                Password = "123456",
                Image = "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Ftse2.mm.bing.net%2Fth%3Fid%3DOIP.cWhfLxi5rmO7bx1axwS9-wHaEK%26pid%3DApi&f=1&ipt=4edddfad96156df5f32b85682385b76e9b829f9ae0d3dd957d8e8f36919272e0&ipo=images"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var result = await _client.PutAsync(urlToCall, stringContent);

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Delete_UserById_ShouldReturnUnauthorized() {
            var urlToCall = new Uri($"{ServiceUrl}/api/v1/users/1");

            var result = await _client.DeleteAsync(urlToCall);

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Create_User_ShouldReturnCreated() {
            var urlToCall = new Uri($"{ServiceUrl}/api/v1/users");

            var payload = new User()
            {
                Name = "guigui",
                Password = "123456",
                Image = "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Ftse2.mm.bing.net%2Fth%3Fid%3DOIP.cWhfLxi5rmO7bx1axwS9-wHaEK%26pid%3DApi&f=1&ipt=4edddfad96156df5f32b85682385b76e9b829f9ae0d3dd957d8e8f36919272e0&ipo=images"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var result = await _client.PostAsync(urlToCall, stringContent);

            result.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Get_Users_Authenticated_ShouldReturnOk() {
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Configuration.Auth.GenerateToken(1));

            var urlToCall = new Uri($"{ServiceUrl}/api/v1/users");

            var result = await _client.GetAsync(urlToCall);

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_UserById_ShouldReturnOk() {
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Configuration.Auth.GenerateToken(1));

            var urlToCall = new Uri($"{ServiceUrl}/api/v1/users/1");

            var result = await _client.GetAsync(urlToCall);

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Put_UserById_UserDoesNotExist_ShouldReturnError() {
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Configuration.Auth.GenerateToken(1));

            var urlToCall = new Uri($"{ServiceUrl}/api/v1/users/1");

            var payload = new User() {
                Name = "guigui123",
                Password = "123456",
                Image = "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Ftse2.mm.bing.net%2Fth%3Fid%3DOIP.cWhfLxi5rmO7bx1axwS9-wHaEK%26pid%3DApi&f=1&ipt=4edddfad96156df5f32b85682385b76e9b829f9ae0d3dd957d8e8f36919272e0&ipo=images"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var result = await _client.PutAsync(urlToCall, stringContent);

            result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task Delete_UserById_UserDoesNotExist_ShouldReturnError() {
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Configuration.Auth.GenerateToken(1));

            var urlToCall = new Uri($"{ServiceUrl}/api/v1/users/1");

            var result = await _client.DeleteAsync(urlToCall);

            result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}
