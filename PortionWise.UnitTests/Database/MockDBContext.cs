using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PortionWise.Database;
using PortionWise.Models.Recipe.Entities;

namespace PortionWise.UnitTests.Database
{
    public class MockDBContext
    {
        public readonly ApplicationDBContext Context;

        public MockDBContext(string databaseName)
        {
            var initialData = new Dictionary<string, string?>
            {
                { "ConnectionStrings:DefaultSQLConnection", "DataSource=:memory:" }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(initialData)
                .Build();

            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            var dbContext = new ApplicationDBContext(configuration);
            dbContext.Database.OpenConnection(); // Open connection for in-memory SQLite
            dbContext.Database.EnsureCreated(); // Ensure that all tables are created

            Context = dbContext;
        }

        public void AddTestingData(List<RecipeEntity> recipes)
        {
            Context.Recipes.AddRange(recipes);
            Context.SaveChanges();
        }

        public void ClearTestingData()
        {
            Context.Recipes.RemoveRange(Context.Recipes);
            Context.SaveChanges();
        }
    }
}
