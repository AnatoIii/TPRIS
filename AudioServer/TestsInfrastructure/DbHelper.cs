using AudioServer.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace TestsInfrastructure
{
    /// <summary>
    /// Helper for <see cref="GameSalesContext"/>
    /// </summary>
    public static class DbHelper
    {
        /// <summary>
        /// Creates test in memory <see cref="GameSalesContext"/> with 
        /// <paramref name="dbName"/> and <paramref name="targetTestData"/>
        /// </summary>
        /// <typeparam name="T">Target type</typeparam>
        /// <param name="dbName">Target DB name</param>
        /// <param name="targetTestData">Target test data</param>
        public static AudioServerDBContext GetTestContextWithTargetParams<T>(string dbName, IEnumerable<T> targetTestData)
            where T : class
        {
            var dbContextOptions = new DbContextOptionsBuilder<AudioServerDBContext>()
                .UseInMemoryDatabase(databaseName: dbName, new InMemoryDatabaseRoot())
                .Options;

            using (var context = new AudioServerDBContext(dbContextOptions, true))
            {
                context.Set<T>().AddRange(targetTestData);
                context.SaveChanges();
            }

            return new AudioServerDBContext(dbContextOptions, true);
        }
    }
}
