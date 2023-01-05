using FluentAssertions;
using Fullstack.API.Controllers;
using Fullstack.API.Interfaces;
using Fullstack.API.Models;
using Fullstack.API.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Configuration;
using UnitTest.Fixtures;
using Xunit;

namespace UnitTest.Services {
    public class UsersService {

        public UsersService() {
        }

        [Fact]
        public async Task Get_Users_ShouldReturnListOfUsers() {
            var mock = new Mock<IUsers>();

            mock.Setup(service => service.GetAll()).ReturnsAsync(UsersFixture.GetTestUsers());

            var users = await mock.Object.GetAll();

            Assert.NotNull(users);
            users.Should().BeOfType<List<User>>();
        }

        [Fact]
        public async Task Get_User_ShouldReturnAUser() {
            var mock = new Mock<IUsers>();
            int userId = 3;

            mock.Setup(service => service.Get(userId)).ReturnsAsync(UsersFixture.GetTestUsers().FirstOrDefault(x => x.Id == userId));

            var user = await mock.Object.Get(userId);

            Assert.NotNull(user);
            user.Should().BeOfType<User>();
            user.Id.Should().Be(userId);
        }

        [Fact]
        public async Task Delete_User_ShouldReturnTrue() {
            var mock = new Mock<IUsers>();
            int userId = 3;

            mock.Setup(service => service.Delete(userId)).ReturnsAsync(true);

            var userDelete = await mock.Object.Delete(userId);

            userDelete.Should().BeTrue();
        }
    }
}
