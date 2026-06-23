using MiniFinancialSupport.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniFinancialSupport.Application.Interface
{
    public interface ITransferService
    {
        Task<TransferResponse> TransferAsync(TransferRequest request, CancellationToken cancellationToken = default);
    }
}
