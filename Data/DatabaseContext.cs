
namespace Tutorial_DotNet.Data;

public class DatabaseContext : DbContext {
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options){
        
    }
    public DbSet<Character> Characters => Set<Character>();
    public DbSet<User> Users => Set<User>();
}