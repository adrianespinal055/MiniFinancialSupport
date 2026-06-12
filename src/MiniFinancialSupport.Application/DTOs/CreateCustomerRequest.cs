namespace MiniFinancialSupport.Application.DTOs;

// DTO de ENTRADA: solo los campos que el cliente puede enviar al crear UN customer.
// Nombre en singular (CreateCustomerRequest) porque crea un solo cliente: convención y consistencia.
// No incluye Id, IsActive ni CreatedAt: esos los pone el sistema, no el cliente.
public class CreateCustomerRequest
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}
