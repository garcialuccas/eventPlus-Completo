using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PresencaController : ControllerBase
    {
        private readonly IInscricao _inscricao;

        public PresencaController(IInscricao inscricao)
        {
            _inscricao = inscricao;
        }




        [HttpGet("Listar")]
        public async Task<IActionResult> Lista()
        {
            try
            {
                var lista = await _inscricao.Listar();

                return Ok(lista);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }

        [HttpGet("ListarPresencaPorId/{id:guid}")]
        public async Task<IActionResult> ListarMinhaPresenca(Guid id)
        {
            try
            {
                var inscricaoEncontrada = await _inscricao.ListarMinhasPresencas(id);

                return Ok(inscricaoEncontrada);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }


        [HttpGet("BuscaPresencaId/{id:guid}")]
        public async Task<IActionResult> BuscarPorID(Guid id)
        {
            try
            {
                var presencaEncontrada = await _inscricao.BuscarPorId(id);

                return Ok(presencaEncontrada);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }



        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> deletar(Guid id) 
        {
            try
            {
                await _inscricao.Deletar(id);
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }


        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] InscricaoDTO inscricaoDTO) 
        {
            var inscricao = new Presenca 
            {
            EventoId = inscricaoDTO.EventoId,
            Situacao = inscricaoDTO.Situacao,
            UsuarioId = inscricaoDTO.UsuarioId
            };

            try
            {
                await _inscricao.Cadastrar(inscricao);
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }


        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id ,[FromBody] InscricaoAtualizarDTO presenca) 
        {
            var presencaAtualizar = new Presenca 
            {
            EventoId = presenca.EventoId,
            Situacao = presenca.Situacao,
            UsuarioId = presenca.UsuarioId
            };

            try
            {
                await _inscricao.AtualizarSituacao(id,presencaAtualizar);
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
