using FluentValidation.Internal;
using Microsoft.EntityFrameworkCore;
using MiniFinancialSupport.Application.DTOs;
using MiniFinancialSupport.Application.Interfaces;
using MiniFinancialSupport.Domain.Entities;
using MiniFinancialSupport.Domain.Exceptions;
using MiniFinancialSupport.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniFinancialSupport.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _db;
        private readonly ICustomerService _customerService;

        public AccountService(ApplicationDbContext db, ICustomerService customerService)
        {
            _db = db;
            _customerService = customerService;
        }

        public async Task<AccountResponse> CreateAsync(CreateAccountRequest request, CancellationToken cancellationToken = default)
        {
            var customer = await _customerService.GetByIdAsync(request.CustomerId);
            if (customer is null)
            {
                throw new BusinessRuleException("Customer not found.");
            }

            if (!customer.IsActive)
            {
                throw new BusinessRuleException("Cannot create an account for a inactive customer");
            }

            var account = new Account
            {
                CustomerId = request.CustomerId,
                AccountNumber = GenerateAccountNumber(),
                Balance = 0m,
                IsBlocked = false,
                CreatedAt = DateTime.UtcNow
            };

            _db.Accounts.Add(account);
            await _db.SaveChangesAsync(cancellationToken);
            return MapToResponse(account);
        }
        public async Task<AccountResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            //Solo lectura para consumir menos recursos
            var account = await _db.Accounts.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return account is null ? null : MapToResponse(account);
        }

        public async Task<AccountResponse> DepositAsync(int id, AmountRequest request, CancellationToken cancellationToken = default)
        {
            //Rule No Deposit
            if (request.Amount <= 0)
            {
                throw new BusinessRuleException("Deposit amount must be greater than 0.");
            }

            var account = await GetTrackedAccountAsync(id, cancellationToken);

            //Regla
            if (account.IsBlocked)
            {
                throw new BusinessRuleException("Account is blocked");
            }

            account.Balance += request.Amount;
            await _db.SaveChangesAsync(cancellationToken);
            return MapToResponse(account);


        }

        public async Task<AccountResponse> WithdrawAsync(int id, AmountRequest request, CancellationToken cancellationToken = default)
        {
            //REGLA: No retirar <=0
            if (request.Amount <= 0)
            {
                throw new BusinessRuleException("Withdraw amount must be greater than 0.");
            }

            var account = await GetTrackedAccountAsync(id, cancellationToken);

            //REGLA: no retirar mas del saldo sin negativos
            if (account.IsBlocked)
            {
                throw new BusinessRuleException("Account is blocked");
            }

            //REGLA: no retirar mas del salfo sin neg
            if (account.Balance < request.Amount)
            {
                throw new BusinessRuleException("Insufficient balance.");
            }

            account.Balance -= request.Amount;
            await _db.SaveChangesAsync(cancellationToken);
            return MapToResponse(account);
        }

        //carga la cuenta rastreada para modificarla o lanza si no existe 
        private async Task<Account> GetTrackedAccountAsync(int id, CancellationToken cancellationToken)
        {
            var account = await _db.Accounts.FirstOrDefaultAsync(a => a.Id == id);
            if (account is null)
            {
                throw new BusinessRuleException("Account is not found");
            }
            return account;
        }

        private static string GenerateAccountNumber() =>
            "ACC" + Guid.NewGuid().ToString("N")[..10].ToUpper();

        private static AccountResponse MapToResponse(Account a) => new()
        {
            Id = a.Id,
            CustomerId = a.CustomerId,
            AccountNumber = a.AccountNumber,
            Balance = a.Balance,
            IsBlocked = a.IsBlocked,
            CreatedAt = a.CreatedAt
        };
    }
}
