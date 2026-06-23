using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniFinancialSupport.Application.DTOs;
using MiniFinancialSupport.Application.Interface;

namespace MiniFinancialSupport.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransfersController : ControllerBase
    {
        private readonly ITransferService _transferService;

        public TransfersController(ITransferService transferService)
        {
            _transferService = transferService;
        }

        //Proceso de transferencia
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TransferResponse>> Transfer(TransferRequest request, CancellationToken cancellationToken)
        {
            var result = await _transferService.TransferAsync(request, cancellationToken);
            return Ok(result);

        }






    }
}
