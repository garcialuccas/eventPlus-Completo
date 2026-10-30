using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoEventoController : ControllerBase
    {
        private readonly ITipoEvento _tipoEvento;
        public TipoEventoController(ITipoEvento tipoEvento)
        {
            _tipoEvento = tipoEvento;
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
                var tipos = await _tipoEvento.Listar();

                return Ok(tipos);
            }
            catch (Exception)
            {
                return BadRequest("Erro !");
            }
        }

        /// <summary>
        /// Cadastra um novo Tipo de Evento
        /// </summary>
        /// <param name="TipoEvento">Tipo do evento a ser cadastrado</param>
        /// <returns>Created e objeto cadastrado </returns>

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] TipoEventoDTO dto)
        {
            var TipoEvento = new TipoEvento
            {
                Titulo = dto.Titulo
            };

            await _tipoEvento.Cadastrar(TipoEvento);

            return StatusCode(201, TipoEvento);
        }

        /// <summary>
        /// Busca Tipo evento por id do Tipo Evento
        /// </summary>
        /// <param name="id">id do tipo evento </param>
        /// <returns>retorna objeto Tipo evento</returns>

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var TipoEventoBuscado = await _tipoEvento.BuscarPorId(id);

            if (TipoEventoBuscado == null)
            {
                return NotFound("Tipo de usuário não encontrado.");
            }

            return Ok(TipoEventoBuscado);
        }

        /// <summary>
        /// Deleta tipo evento por id do tipo evento
        /// </summary>
        /// <param name="id"> id do tipo evento </param>
        /// <returns> Sucesso </returns>


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletarPorId(Guid id)
        {
            await _tipoEvento.Deletar(id);
            return StatusCode(200);
        }

        /// <summary>
        /// Atualiza o titulo do tipo evento pelo id do tipo evento
        /// </summary>
        /// <param name="TipoEvento"> Objeto do tipo evento </param>
        /// <param name="id"> id do tipo evento </param>
        /// <returns> Sucesso </returns>


        [HttpPut("{id:guid}")]
        public async Task<IActionResult> AtualizarTituloPorId(TipoEvento TipoEvento, Guid id)
        {
            await _tipoEvento.Atualizar(id, TipoEvento);

            return StatusCode(200);
        }
    }
}

