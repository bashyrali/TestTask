using Microsoft.EntityFrameworkCore;
using TestApp.Models.Entity;

namespace TestApp.Data
{
    public class MyAppContext :DbContext
    {
        public MyAppContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ClientInfo> ClientInfos { get; set; }
    }
}