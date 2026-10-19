using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model;

namespace DAL
{
    public class MyDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Borrowing> Borrowings { get; set; }
        public DbSet<Piece> Pieces { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Title> Titles { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
