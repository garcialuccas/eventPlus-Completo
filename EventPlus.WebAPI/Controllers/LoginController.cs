using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventPlus.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsuario _repository;
        private readonly IConfiguration _configurantion;

        public LoginController(IUsuario repository, IConfiguration configuration)
        {
            _repository = repository;
            _configurantion = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> BuscarPorEmail([FromBody] LoginDTO dto)
        {
            try
            {
                var u = await _repository.BuscaPorEmaileSenha(dto.Email);
                if (u == null || !CriptografarSenha.VerificarSenha(dto.Senha, u.Senha))
                {
                    return Unauthorized("Email ou senha inválidos");
                }

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, u.UsuarioId.ToString()),

                    new Claim("NomeUsuario", u.Nome),

                    new Claim(JwtRegisteredClaimNames.Email, u.Email),

                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var chaveSecreta = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurantion["Jwt:Key"]!));

                var credenciais = new SigningCredentials(chaveSecreta, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(issuer: "EventPlus.WebAPI", audience: "EventPlus.WebAPI", claims: claims, expires: DateTime.UtcNow.AddHours(8), signingCredentials: credenciais);

                string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return StatusCode(200, new { Token = tokenString, Expiracao = token.ValidTo, Usuario = new { u.UsuarioId, u.Nome, u.Email } });
            }
            catch
            {
                return StatusCode(404);
            }
        }
    }
}