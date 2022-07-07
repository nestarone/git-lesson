using Microsoft.EntityFrameworkCore;
using ToDoList.Domain;

namespace ToDoList.Infrastructure.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        //protected DatabaseContext()
        //{
        //}

        public DbSet<User> Users{get; set;}
    }
}