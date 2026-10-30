using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using EventPlus.WebAPI.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.Arm;

namespace EventPlus.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuario _Usuario;

        public UsuarioController(IUsuario usuario)
        {
            _Usuario = usuario;
        }


        [HttpGet("Listar")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var listaUsuario = await _Usuario.Listar();

                return Ok(listaUsuario);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }

        }


        [HttpGet("{id:GUID}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var usuarioEncontrado = await _Usuario.BuscaPorId(id);

            if (usuarioEncontrado == null)
            {
                return NotFound();
            }

            return StatusCode(200, usuarioEncontrado);
        }


        [HttpGet]
        public async Task<IActionResult> BuscaPorEmaileSenha([FromBody] LoginDTO loginDTO)
        {
            var usuarioEncontrado = await _Usuario.BuscaPorEmaileSenha(loginDTO.Email);

            if (usuarioEncontrado == null)
            {
                return NotFound();
            }

            if (!CriptografarSenha.VerificarSenha(loginDTO.Senha, usuarioEncontrado.Senha))
            {
            return StatusCode(404, "Usuario ou senha incorretos");
            }

            return Ok("Login Realizado");
            
        }


        [HttpPost]
        public async Task<IActionResult> Cadastro(UsuarioDTO usuarioDTO) 
        {
            Usuario usuario = new Usuario
            {
                Nome = usuarioDTO.Nome,
                Senha = CriptografarSenha.Criptografarsenha(usuarioDTO.Senha),
                Email = usuarioDTO.Email,
                TipoUsuarioId = usuarioDTO.TipoUsuarioid
            };

            try
            {
            await _Usuario.Cadastrar(usuario);
            return Ok("Cadastrado com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest("Esse email ja esta sendo utilizado no sistema.");
                throw;
            }
        }




        [HttpPatch("{id:GUID}")]
        public async Task<IActionResult> AtualizarUsuario(Guid id, [FromBody] UsuarioAtualizarDTO usuarioDTO) 
        {

            Usuario usuario = new Usuario 
            {
            TipoUsuarioId = usuarioDTO.TipoUsuarioid,
            Nome = usuarioDTO.Nome,
            Senha = usuarioDTO.Senha,
            Email = usuarioDTO.Email
            };

            try
            {

            await _Usuario.Atualizar(id, usuario);
            return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }


        [HttpDelete("{id:GUID}")]
        public async Task<IActionResult> DeletarUsuario(Guid id) 
        {
            try
            {
                await _Usuario.Deletar(id);
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }


        [HttpPost("NovaPresenca/{idevento:guid}/{idusuario:guid}")]
        public async Task<IActionResult> NovaPresenca(Guid idevento, Guid idusuario) 
        {
            try
            {
                await _Usuario.NovaPresenca(idevento, idusuario);
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }
    }
}
