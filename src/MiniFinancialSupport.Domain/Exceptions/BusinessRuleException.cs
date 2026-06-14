namespace MiniFinancialSupport.Domain.Exceptions;

// Excepción para VIOLACIONES DE REGLAS DE NEGOCIO
// (ej: saldo insuficiente, cuenta bloqueada, depósito <= 0).
// Los servicios la lanzan; el middleware la mapea a 400 Bad Request con el mensaje.
public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message)
    {
    }
}
