using Microsoft.EntityFrameworkCore;
using AudioServer.Models;

namespace AudioServer.DataAccess
{
    public class AudioServerDBContext : DbContext
    {
        public DbSet<FileEntity> Files { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }

        public AudioServerDBContext(DbContextOptions options) : base(options)
        { }
    }
}