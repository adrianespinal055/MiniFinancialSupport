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
    [Authorize(Roles = "Admin")] // Con esta linea solo admin tiene acceso a crear 
    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Create(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var created = await _customerService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetByIdAsync(id, cancellationToken);

        if (customer is null)
        {
            return NotFound();
        }
        return Ok(customer);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<CustomerResponse>> Update(int id, CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var updated = await _customerService.UpdateAsync(id, request, cancellationToken);
        if (updated is null) return NotFound();
        return Ok(updated);

    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id:int}/inactive")]
    public async Task<IActionResult> Inactive(int id, CancellationToken cancellationToken = default)
    {
        var ok = await _customerService.InactivateAsync(id, cancellationToken);
        if (!ok) return NotFound();
        return NoContent();
    }

}
