using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AuthorizationService.DAL.Context;

public class AuthorizationContextFactory : IDesignTimeDbContextFactory<AuthorizationContext>
{
    public AuthorizationContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AuthorizationContext>();

        optionsBuilder.UseSqlServer("Data Source=localhost,1337;Initial Catalog=AuthorizationSystem;User Id=SA; Password=msSQL123;TrustServerCertificate=True;");
        return new AuthorizationContext(optionsBuilder.Options);
    }
}