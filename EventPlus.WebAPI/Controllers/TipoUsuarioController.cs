using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoUsuarioController : ControllerBase
    {
        private readonly ITipoUsuario _TipoUsuario;
        public TipoUsuarioController(ITipoUsuario TipoUsuario)
        {
            _TipoUsuario = TipoUsuario;
        }

        /// <summary>
        /// Lista todos os Eventos
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var tipos = await _TipoUsuario.Listar();

                return Ok(tipos);
            }
            catch (Exception)
            {
                return BadRequest("Erro !");
            }
        }

        /// <summary>
        /// Cadastra um novo perfil de usuário
        /// </summary>
        /// <param name="TipoUsuario">Perfil do usuario a ser cadastrado</param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] TipoUsuarioDTO dto)
        {
            var TipoUsuario = new TipoUsuario
            {
                Titulo = dto.Titulo
            };

            await _TipoUsuario.Cadastrar(TipoUsuario);

            return StatusCode(201, TipoUsuario);
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var TipoUsuarioBuscado = await _TipoUsuario.BuscarPorId(id);

            if (TipoUsuarioBuscado == null)
            {
                return NotFound("Tipo de usuário não encontrado.");
            }

            return Ok(TipoUsuarioBuscado);
        }



        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletarPorId(Guid id)
        {
            await _TipoUsuario.Deletar(id);
            return StatusCode(200);
        }



        [HttpPut("{id:guid}")]
        public async Task<IActionResult> AtualizarTituloPorId(TipoUsuario TipoUsuario, Guid id)
        {
            await _TipoUsuario.Atualizar(id, TipoUsuario);

            return StatusCode(200);
        }


    }
}
