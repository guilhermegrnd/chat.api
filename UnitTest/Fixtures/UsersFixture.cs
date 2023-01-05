using Fullstack.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Fixtures {
    public class UsersFixture {
        public static List<User> GetTestUsers() => new() {
            new User{ Id = 1, Name = "guigui" },
            new User{ Id = 2, Name = "gab" },
            new User{ Id = 3, Name = "vardo" }
        };
    }
}
