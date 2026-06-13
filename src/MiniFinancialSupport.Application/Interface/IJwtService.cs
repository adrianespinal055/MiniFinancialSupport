namespace MiniFinancialSupport.Application.Interfaces;

// Contrato del servicio que genera tokens JWT.
// Devuelve el token (string) y cuándo expira (DateTime).
public interface IJwtService
{
    (string Token, DateTime ExpiresAtUtc) GenerateToken(string email, string role);
}
