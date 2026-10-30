using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstituicaoController : ControllerBase
    {
        private readonly IInstituicao _instituicao;

        public InstituicaoController(IInstituicao instituicao)
        {
            _instituicao = instituicao;
        }



        [HttpGet("Listar")]
        public async Task<IActionResult> Listar() 
        {
            try
            {
                var instituicao = await _instituicao.Listar();
                return Ok(instituicao);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }



        [HttpPost("Cadastro/{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] InstituicaoDTO instituicaoDTO) 
        {
            var instituicao = new Instituicao
            {
                Cnpj = instituicaoDTO.CNPJ,
                Endereco = instituicaoDTO.Endereco,
                NomeFantasia = instituicaoDTO.NomeFantasia
            };

            try
            {
                await _instituicao.Atualizar(id,instituicao);
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }



        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] InstituicaoDTO instituicao) 
        {
            var instituicaoDTO = new Instituicao 
            {
            Cnpj = instituicao.CNPJ,
            Endereco = instituicao.Endereco,
            NomeFantasia = instituicao.NomeFantasia
            };


            try
            {
            await _instituicao.Cadastrar(instituicaoDTO);
            return StatusCode(200, instituicao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
                throw;
            }
       }



        [HttpGet("{id:guid}")]
        public async Task<IActionResult> BuscarPorId(Guid id) 
        {
            try
            {
                var InstituicaoEncontrado = await _instituicao.BuscarPorId(id);
                return StatusCode(200, InstituicaoEncontrado);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }
    }
}
