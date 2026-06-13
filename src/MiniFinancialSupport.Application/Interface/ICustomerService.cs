using MiniFinancialSupport.Application.DTOs;

namespace MiniFinancialSupport.Application.Interfaces;

// El "contrato": qué sabe hacer el servicio, sin decir CÓMO.
public interface ICustomerService
{
    Task<CustomerResponse> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default);
    Task<List<CustomerResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<CustomerResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}