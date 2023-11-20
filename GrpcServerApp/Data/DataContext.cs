using Microsoft.EntityFrameworkCore;

namespace GrpcServerApp.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    public DbSet<ToDo> ToDos { get; set; } = null!;
}