using Microsoft.EntityFrameworkCore;
using MiniFinancialSupport.Application.DTOs;
using MiniFinancialSupport.Application.Interfaces;
using MiniFinancialSupport.Domain.Entities;
using MiniFinancialSupport.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniFinancialSupport.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {

        private readonly ApplicationDbContext _db;

        //El DBContext llega por inyeccion de dependencias

        public CustomerService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<CustomerResponse> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
        {
            //Mapear DTO de entrada -> entidad Controlamos que se guarda
            var customer = new Customer
            {
                FullName = request.FullName,
                Email = request.Email,
                DocumentNumber = request.DocumentNumber,
                Phone = request.Phone,
                IsActive = true,//Se crea Activo
                CreatedAt = DateTime.UtcNow
            };

            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();

            return MapToResponse(customer);
        }

        public async Task<List<CustomerResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            //As no tracking solo lectura, mas rapido(EF no rastrea cambios
            var customers = await _db.Customers.AsNoTracking()
                .ToListAsync(cancellationToken);

            return customers.Select(MapToResponse).ToList();
        }

        private static CustomerResponse MapToResponse(Customer c) => new()
        {
            Id = c.Id,
            FullName = c.FullName,
            Email = c.Email,
            DocumentNumber = c.DocumentNumber,
            Phone = c.Phone,
            IsActive = c.IsActive,
            CreatedAt = c.CreatedAt
        };

        public async Task<CustomerResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var customer = await _db.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            return customer is null ? null : MapToResponse(customer);
        }
    }
}
