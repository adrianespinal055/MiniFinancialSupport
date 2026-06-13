using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniFinancialSupport.Application.DTOs;
using MiniFinancialSupport.Application.Interfaces;
using MiniFinancialSupport.Infrastructure.Auth;

namespace MiniFinancialSupport.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        public AuthController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public ActionResult<LoginResponse> Login(LoginRequest request)
        {
            //Buscar usuario por email + contrasena en los usuarios de prueba
            var user = SeedUsers.All.FirstOrDefault(Usuario =>
            Usuario.Email == request.Email && Usuario.Password == request.Password);

            //Si no exite o  el pass no coincide -401
            //Secure coding: No decimos si fallo el email o el pass, no damos pista a atacantes

            if (user is null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }
            //Generamos el token con su email y su rol

            var (token, expiresAtUtc) = _jwtService.GenerateToken(user.Email, user.Role);

            return Ok(new LoginResponse
            {
                Token = token,
                Email = user.Email,
                Role = user.Role,
                ExpiresAtUtc = expiresAtUtc
            });
        }
    }
}
