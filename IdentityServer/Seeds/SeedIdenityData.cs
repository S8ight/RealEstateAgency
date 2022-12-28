using System.Security.Claims;
using IdentityServer.Configuration;
using IdentityServer.Context;
using IdentityServer.Entities;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Seeds;

public class SeedIdenityData
{
     public static void SeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<DbContextIdentity>(
                options => options.UseSqlServer(connectionString)
            );

            services
                .AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<DbContextIdentity>()
                .AddDefaultTokenProviders();

            services.AddOperationalDbContext(
                options =>
                {
                    options.ConfigureDbContext = db =>
                        db.UseSqlServer(
                            connectionString,
                            sql => sql.MigrationsAssembly(typeof(SeedIdenityData).Assembly.FullName)
                        );
                }
            );
            services.AddConfigurationDbContext(
                options =>
                {
                    options.ConfigureDbContext = db =>
                        db.UseSqlServer(
                            connectionString,
                            sql => sql.MigrationsAssembly(typeof(SeedIdenityData).Assembly.FullName)
                        );
                }
            );

            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            scope.ServiceProvider.GetService<PersistedGrantDbContext>()?.Database.Migrate();
            
            var dbContext = scope.ServiceProvider.GetService<ConfigurationDbContext>();
            dbContext?.Database.Migrate();
            EnsureSeedData(dbContext);
            
            var identityServerDatabaseContext = scope.ServiceProvider.GetService<DbContextIdentity>();
            identityServerDatabaseContext?.Database.Migrate();
            EnsureUsers(scope);
        }

        private static void EnsureUsers(IServiceScope scope)
        {
            var manager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            var user = manager.FindByNameAsync("Gianluigi").Result;
            if (user == null)
            {
                user = new User()
                {
                    UserName = "Gianluigi",
                    FirstName = "Buffon",
                    LastName = "Gianluigi",
                    Patronymic = "Buffoonish",
                    Email = "buf.luigi@gmail.com",
                    DateOfBirthd = DateTime.Now,
                    EmailConfirmed = true
                };
                var result = manager.CreateAsync(user, "Pass!12345").Result;
                result =
                    manager.AddClaimsAsync(
                        user,
                        new Claim[]
                        {
                            new Claim("role", "User")
                        }
                    ).Result;
            }

            var testAdmin = manager.FindByNameAsync("buffAdmin").Result;
            if (testAdmin == null)
            {
                testAdmin = new User()
                {
                    UserName = "buffAdmin",
                    FirstName = "Buffon",
                    LastName = "Gianluigi",
                    Patronymic = "Buffoonish",
                    Email = "buffon.luigi@gmail.com",
                    DateOfBirthd = DateTime.Now,
                    EmailConfirmed = true
                };
                var result = manager.CreateAsync(testAdmin, "Pass!12345").Result;

                result =
                    manager.AddClaimsAsync(
                        testAdmin,
                        new Claim[]
                        {
                            new Claim("role", "Admin")
                        }
                    ).Result;
            }
        }

        private static void EnsureSeedData(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                foreach (var client in Config.GetClients())
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.GetIdentityResources.ToList())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            // if (!context.ApiScopes.Any())
            // {
            //     foreach (var resource in Config.GetApiScopes.ToList())
            //     {
            //         context.ApiScopes.Add(resource.ToEntity());
            //     }
            //
            //     context.SaveChanges();
            // }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in Config.GetApiResources())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
        }
}