using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace REA.ChatSystem.DAL.Context;

public class AgencyContextFactory : IDesignTimeDbContextFactory<AgencyContext>
{
    public AgencyContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AgencyContext>();
        optionsBuilder.UseSqlServer("Data Source=localhost,1337;Initial Catalog=AgencyChatSystem;User Id=SA; Password=msSQL123;TrustServerCertificate=True;");
        return new AgencyContext(optionsBuilder.Options);
    }
}