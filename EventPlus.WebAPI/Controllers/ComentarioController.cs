using CloudinaryDotNet.Actions;
using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentarioController : ControllerBase
    {
        private readonly IComentario _comentario;
        private readonly IModerationService _moderationService;

        public ComentarioController(IComentario comentario)
        {
            _comentario = comentario;
                    }



        [HttpGet("ListarOcultos")]
        public async Task<IActionResult> ListarOcultos()
        {
            try
            {
                var lista = await _comentario.ListarOcultos();
                return Ok(lista);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }


        [HttpGet("Listar")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var lista = await _comentario.Listar();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }



        [HttpGet("BuscarPorId/{id:guid}")]
        public async Task<IActionResult> BuscarPorid(Guid id)
        {
            try
            {
                var buscarPorID = await _comentario.BuscarPorId(id);
                return Ok(buscarPorID);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] ComentarioDTO comentarioDTO)
        {
          
            try
            {
                //bool reprovado = await _moderationService.moderarTexto(comentarioDTO.ComentarioUsuario);

                var comentario = new Comentario
                {
                    DataComentario = comentarioDTO.DataComentario,
                    EventoId = comentarioDTO.EventoId,
                    UsuarioId = comentarioDTO.UsuarioId,
                    ComentarioUsuario = comentarioDTO.ComentarioUsuario,
                    Exibe = true
                };

                await _comentario.Cadastrar(comentario);
                return Ok("Cadastro realizado com sucesso");
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                await _comentario.Deletar(id);

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }

        [HttpGet("ListarPorEventoId/{id:guid}")]
        public async Task<IActionResult> ListarPorEventoId(Guid id)
        {
            try
            {
                var eventoEncontrado = await _comentario.ListarPorEvento(id);
                return Ok(eventoEncontrado);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }



        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> AtualizarExibe(Guid id, [FromBody] ComentarioAtualizarDTO comentarioAtualizarDTO)
        {
            var atualizaExibe = new Comentario
            {
                Exibe = comentarioAtualizarDTO.Exibe
            };

            try
            {
                await _comentario.EditarVisible(id, atualizaExibe);

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
