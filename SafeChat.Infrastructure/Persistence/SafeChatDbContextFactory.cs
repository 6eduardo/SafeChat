using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SafeChat.Infrastructure.Persistence
{
    public class SafeChatDbContextFactory : IDesignTimeDbContextFactory<SafeChatDbContext>
    {
        public SafeChatDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("Default");

            var optionsBuilder = new DbContextOptionsBuilder<SafeChatDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new SafeChatDbContext(optionsBuilder.Options);
        }
    }
}
