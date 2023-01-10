using Chat.API.Data;
using Chat.API.Interfaces;
using Chat.API.Models;
using Chat.API.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Configuration
{
    [ExcludeFromCodeCoverage]
    public class ServerStartup : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            var root = new InMemoryDatabaseRoot();

            builder.ConfigureServices(services =>
            {
                /*
                services
                    .AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = IntegrationTestAuthenticationHandler.AuthenticationScheme;
                        options.DefaultScheme = IntegrationTestAuthenticationHandler.AuthenticationScheme;
                        options.DefaultChallengeScheme = IntegrationTestAuthenticationHandler.AuthenticationScheme;
                    })
                    .AddScheme<AuthenticationSchemeOptions, IntegrationTestAuthenticationHandler>(
                        IntegrationTestAuthenticationHandler.AuthenticationScheme,
                        options => { }
                    );
                //AUTH
                var tokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("123456789987654321")),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
                services.AddSingleton(tokenValidationParameters);
                services.AddAuthentication(x => {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = tokenValidationParameters;
                });
                */
                services.AddScoped(sp =>
                {
                    return new DbContextOptionsBuilder<DataContext>()
                        .UseInMemoryDatabase("Tests", root)
                        .UseApplicationServiceProvider(sp)
                        .Options;
                });
                
                //services.AddControllers();
                //services.AddEndpointsApiExplorer();
                //services.AddScoped(x => new Mock<IAuth>().Object);
                services.AddScoped(x => new Mock<IUsers>().Object);
                //services.AddScoped(x => new Mock<IChat>().Object);
            });

            return base.CreateHost(builder);
        }
    }
}
