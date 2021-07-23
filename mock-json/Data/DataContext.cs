using Microsoft.EntityFrameworkCore;
using mock_json.Entities;

namespace mock_json.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<MockData> MockData { get; set; }
    }
}