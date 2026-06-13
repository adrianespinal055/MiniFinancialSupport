namespace MiniFinancialSupport.Infrastructure.Auth;

// Un usuario de prueba (email, contraseña, rol).
public record SeedUser(string Email, string Password, string Role);

// Usuarios de prueba EN MEMORIA.
// En una app real vendrían de la BD y la contraseña estaría HASHEADA (ej. BCrypt), nunca en claro.
public static class SeedUsers
{
    public static readonly IReadOnlyList<SeedUser> All = new List<SeedUser>
    {
        new("admin@test.com",   "Admin123*",   "Admin"),
        new("qa@test.com",      "Qa123*",      "QA"),
        new("support@test.com", "Support123*", "Support"),
        new("user@test.com",    "User123*",    "User"),
    };
}
