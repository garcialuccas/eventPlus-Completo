using CloudinaryDotNet;
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
    public class EventoController : ControllerBase
    {
        private readonly IEvento _Evento;
        private readonly Cloudinary _Cloudinary;

        public EventoController(IEvento evento, IConfiguration config)
        {
            _Evento = evento;

            var account = new Account(
                  config["Cloudinary:CloudName"],
                  config["Cloudinary:ApiKey"],
                  config["Cloudinary:ApiSecret"]
              );
            _Cloudinary = new Cloudinary(account);
        }



        [HttpGet("ListarEvento")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var listaEvento = await _Evento.Listar();

                return Ok(listaEvento);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }



        [HttpGet("{id:guid}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            try
            {
                var EventoEncontrado = await _Evento.BuscarPorId(id);

                return Ok(EventoEncontrado);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }


        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromForm] EventoDTO eventoDTO)
        {
            try
            {
            string urlImagem = null;

                if (eventoDTO.Imagem != null && eventoDTO.Imagem.Length > 0)
                {
                    await using var stream = eventoDTO.Imagem.OpenReadStream();
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(eventoDTO.Imagem.FileName, stream),
                        Folder = "eventplus/eventos"
                    };

                    var uploadResult = await _Cloudinary.UploadAsync(uploadParams);
                    urlImagem = uploadResult.SecureUrl.ToString();
                }

                Evento evento = new Evento
                {
                    TipoEventoId = eventoDTO.TipoEventoId,
                    InstituicaoId = eventoDTO.InstituicaoId,
                    DataEvento = eventoDTO.DataEvento,
                    Titulo = eventoDTO.Titulo,
                    Descricao = eventoDTO.Descricao,
                    ImagemUrl = urlImagem

                };
                 
                await _Evento.Cadastro(evento);
                return StatusCode(200, "Evento cadastrado com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
                throw;
            }
        }


        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] EventoAtualizarDTO eventoAtualizarDTO)
        {

            Evento evento = new Evento
            {
                TipoEventoId = eventoAtualizarDTO.TipoEventoId,
                InstituicaoId = eventoAtualizarDTO.InstituicaoId,
                DataEvento = eventoAtualizarDTO.DataEvento,
                Descricao = eventoAtualizarDTO.Descricao,
                Titulo = eventoAtualizarDTO.Titulo,
                ImagemUrl = eventoAtualizarDTO.ImagemUrl
            };

            try
            {
                await _Evento.Atualizar(id, evento);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
                throw;
            }
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                await _Evento.Deletar(id);
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }


        [HttpGet("ListarPorInstituicao/{id:guid}")]
        public async Task<IActionResult> listarInstituicao(Guid id) 
        {

            try
            {
                var instituicao = await _Evento.ListarPoInstituicao(id);
                return Ok(instituicao);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }



        [HttpGet("ListarPorInscrito/{id:guid}")]
        public async Task<IActionResult> ListarPorInscrito(Guid id)
        {

            try
            {
                var instituicao = await _Evento.ListarPorInscrito(id);
                return Ok(instituicao);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }



        [HttpGet("ListarPorData")]
        public async Task<IActionResult> ListarData()
        {

            try
            {
                var instituicao = await _Evento.ListarProximos();
                return Ok(instituicao);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }




    }
}
