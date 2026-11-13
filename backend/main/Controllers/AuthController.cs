using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Interfaces;

namespace main.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                var result = await _authService.RegisterAsync(dto);

                if (!result.Succeeded)
                {
                    return BadRequest(new
                    {
                        Message = "Rejestracja nie powiodła się.",
                        Errors = result.Errors.Select(e => new { e.Code, e.Description })
                    });
                }

                return StatusCode(StatusCodes.Status201Created, new
                {
                    Message = "Rejestracja zakończona sukcesem."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas rejestracji użytkownika {Email}", dto?.Email);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "Wystąpił wewnętrzny błąd serwera podczas rejestracji."
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var response = await _authService.LoginAsync(dto);

                if (response == null)
                {
                    return Unauthorized(new
                    {
                        Message = "Nieprawidłowy email lub hasło."
                    });
                }

                return Ok(response);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("zablokowane"))
            {
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas logowania użytkownika {Email}", dto?.Email);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "Wystąpił wewnętrzny błąd serwera podczas logowania."
                });
            }
        }
    }
}
