using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniFinancialSupport.Application.DTOs;
using MiniFinancialSupport.Application.Interfaces;

namespace MiniFinancialSupport.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Create(CreateAccountRequest request, CancellationToken cancellationToken)
        {
            var created = await _accountService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { Id = created.Id }, created);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AccountResponse>> GetById(int id, CancellationToken cancellationToken)
        {
            var account = await _accountService.GetByIdAsync(id, cancellationToken);
            if (account is null)
            {
                return NotFound();//404
            }
            return Ok(account);//200
        }

        [HttpPost("{id:int}/deposit")]
        public async Task<ActionResult<AccountResponse>> Deposit(int id, AmountRequest request, CancellationToken cancellationToken)
        {
            var account = await _accountService.DepositAsync(id, request, cancellationToken);
            if (account is null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        [HttpPost("{id:int}/withdraw")]
        public async Task<ActionResult<AccountResponse>> Withdraw(int id, AmountRequest request, CancellationToken cancellationToken)
        {
            var account = await _accountService.WithdrawAsync(id, request, cancellationToken);
            return Ok(account);

        }
    }
}
