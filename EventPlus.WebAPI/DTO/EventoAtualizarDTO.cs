using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class EventoAtualizarDTO
    {


        public Guid? TipoEventoId { get; set; }

        public Guid? InstituicaoId { get; set; }

        public DateTime DataEvento { get; set; } 

        [StringLength(80, ErrorMessage = "O Titulo do evento deve conter no maximo 80 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public string? ImagemUrl { get; set; } = string.Empty;

    }
}
