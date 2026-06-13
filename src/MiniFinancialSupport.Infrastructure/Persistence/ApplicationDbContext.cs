using Microsoft.EntityFrameworkCore;
using MiniFinancialSupport.Domain.Entities;

namespace MiniFinancialSupport.Infrastructure.Persistence;

// DbContext: el puente entre mis entidades C# y la base de datos.
// Hereda de DbContext (de EF Core); por eso puede crear/leer tablas.
public class ApplicationDbContext : DbContext
{
    // EF nos inyecta las "opciones" (incluido el connection string) por el constructor.
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Cada DbSet = una tabla. Este es la tabla "Customers".
    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Account> Accounts => Set<Account>();

}
