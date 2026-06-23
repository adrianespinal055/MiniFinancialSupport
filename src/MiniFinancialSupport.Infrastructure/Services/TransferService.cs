using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using MiniFinancialSupport.Application.DTOs;
using MiniFinancialSupport.Application.Interface;
using MiniFinancialSupport.Application.Interfaces;
using MiniFinancialSupport.Domain.Exceptions;
using MiniFinancialSupport.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniFinancialSupport.Infrastructure.Services
{
    public class TransferService : ITransferService
    {
        private readonly ApplicationDbContext _db;
        private readonly IAccountService _account;
        public TransferService(ApplicationDbContext db, IAccountService account)
        {
            _db = db;
            _account = account;
        }

        public async Task<TransferResponse> TransferAsync(TransferRequest request, CancellationToken cancellationToken = default)
        {
            if (request.Amount <= 0)
            {
                throw new BusinessRuleException("Transfer amount must be greater than 0");
            }

            if (request.FromAccountId == request.ToAccountId)
            {
                throw new BusinessRuleException("Cannot transfer to the same account");
            }

            var fromAccount = await _db.Accounts.FirstOrDefaultAsync(c => c.Id == request.FromAccountId, cancellationToken);

            if (fromAccount is null)
            {
                throw new BusinessRuleException("Source account not found");
                ;
            }

            var toAccount = await _db.Accounts.FirstOrDefaultAsync(a => a.Id == request.ToAccountId, cancellationToken);

            if (toAccount is null)
            {
                throw new BusinessRuleException("Destination account not found");
            }

            if (fromAccount.IsBlocked)
            {
                throw new BusinessRuleException("Source account is blocked");
            }

            if (fromAccount.Balance < request.Amount)
            {
                throw new BusinessRuleException("Insufficient balance");
            }

            if (toAccount.IsBlocked)
            {
                throw new BusinessRuleException("Destination account is blocked");
            }

            fromAccount.Balance -= request.Amount;
            toAccount.Balance += request.Amount;

            await _db.SaveChangesAsync(cancellationToken);

            return new TransferResponse
            {
                FromAccountId = request.FromAccountId,
                ToAccountId = request.ToAccountId,
                Amount = request.Amount,
                Description = request.Description,
                FromBalance = fromAccount.Balance,
                ToBalance = toAccount.Balance
            };



        }
    }
}
