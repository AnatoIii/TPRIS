using Microsoft.EntityFrameworkCore;
using AudioServer.Models;
using System.Linq;

namespace AudioServer.DataAccess
{
    public class AudioServerDBContext : DbContext
    {
        public DbSet<FileEntity> Files { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }

        /// <summary>
        /// Default ctor
        /// </summary>
        /// <param name="options"><see cref="DbContextOptions{TContext}"/></param>
        /// <param name="inMemory">Use inMemory DB</param>
        public AudioServerDBContext(DbContextOptions<AudioServerDBContext> options, bool inMemory = false)
            : base(options)
        {
            if (!inMemory && Database.GetPendingMigrations().Any())
                Database.Migrate();
        }
    }
}