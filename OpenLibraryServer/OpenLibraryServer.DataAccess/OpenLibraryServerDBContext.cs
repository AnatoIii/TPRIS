using System;
using Microsoft.EntityFrameworkCore;
using OpenLibraryServer.Models;

namespace OpenLibraryServer.DataAccess
{
    public class OpenLibraryServerDBContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<BookGenres> BookGenres { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<UserChats> UserChats { get; set; }

        public OpenLibraryServerDBContext()
        {
        }

        public OpenLibraryServerDBContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>().HasOne<ChatMessage>(c => c.ChatMessage).WithOne(cm => cm.Chat)
               .HasForeignKey<Chat>(c => c.ChatMessageId);
            base.OnModelCreating(modelBuilder);
        }
    }
}