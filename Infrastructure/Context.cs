using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure
{
    public class Context : DbContext
    {
        public DbSet<Server> Servers { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connectionString = "server=localhost;user=root;database=productivebot;port=3306;Connect Timeout=5;";
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));
            options.UseMySql(connectionString, serverVersion);

        } 
    }
    public class Server
    {
        public ulong Id { get; set; }
        public string Prefix { get; set; }
        public ulong UserId { get; set; }
        public int CommitmentNumber { get; set; }
    }
}
