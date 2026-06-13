using Microsoft.AspNetCore.Mvc;
using MiniFinancialSupport.Application.DTOs;
using MiniFinancialSupport.Application.Interfaces;
using Microsoft.AspNetCore.Authorization; 

namespace MiniFinancialSupport.Api.Controllers;
[Authorize]//Exige login para todo
[ApiController]                       // activa validación automática y comportamiento de API
[Route("api/customers")]             // ruta base de este controller
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    // Dependemos de la INTERFAZ, no de la clase concreta (inyección de dependencias).
    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    // GET /api/customers  -> 200 OK con la lista
    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var customers = await _customerService.GetAllAsync(cancellationToken);
        return Ok(customers);
    }

    // POST /api/customers -> 201 Created con el customer creado
    [Authorize(Roles ="Admin")] // Con esta linea solo admin tiene acceso a crear 
    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Create(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var created = await _customerService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
    }
}
