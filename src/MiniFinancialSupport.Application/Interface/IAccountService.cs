using MiniFinancialSupport.Application.DTOs;

namespace MiniFinancialSupport.Application.Interfaces;

// Contrato del servicio de cuentas.
public interface IAccountService
{
    Task<AccountResponse> CreateAsync(CreateAccountRequest request, CancellationToken cancellationToken = default);
    Task<AccountResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<AccountResponse> DepositAsync(int id, AmountRequest request, CancellationToken cancellationToken = default);
    Task<AccountResponse> WithdrawAsync(int id, AmountRequest request, CancellationToken cancellationToken = default);
}
