using ExternalService.Common;
using ExternalService.Data;
using ExternalService.Helpers.Auth;
using ExternalService.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ExternalService.Controllers
{
    [EnableCors("PolicyCore")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthHelper _authHelper;
        private readonly DataContext _context;

        public AuthController(IAuthHelper authHelper, DataContext context)
        {
            _authHelper = authHelper;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> GetToken([FromBody] LoginModel model)
        {
            try
            {
                var validationErrors = DataValidator.ValidateModel(model);

                if (validationErrors.Any())
                {
                    return BadRequest(new { message = string.Join(", ", validationErrors) });
                }

                var user = await _context.tblUser.FirstOrDefaultAsync(u => u.StrUserName == model.User);
                if (user == null)
                {
                    return BadRequest(new { message = "Usuario inválido" });
                }

                byte[] hashedPassword = _authHelper.CalculatePasswordHash(user.Id, model.Password);

                // Validar la contraseña
                if (!hashedPassword.SequenceEqual(user.HsPassword))
                {
                    return BadRequest(new { message = $"Contraseña incorrecta." });
                }
                else
                {
                    return Ok(new
                    {
                        Token = _authHelper.CreateToken(new[] { new Claim("User_Id", user.Id.ToString()) }, TimeSpan.FromMinutes(1))
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Ha ocurrido un error, vuelva a intentarlo o contacte con su administrador" });
            }
        }
    }
}
