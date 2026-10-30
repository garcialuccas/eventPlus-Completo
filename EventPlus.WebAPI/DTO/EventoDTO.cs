using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class EventoDTO
    {
        [Required(ErrorMessage = "O Tipo do evento é obrigatório.")]
        public Guid? TipoEventoId { get; set; }


        [Required(ErrorMessage = "A Instituição é obrigatória.")]
        public Guid? InstituicaoId { get; set; }

        [Required(ErrorMessage = "A data do evento é obrigatória.")]
        public DateTime DataEvento { get; set; }


        [Required(ErrorMessage = "O Titulo do evento é obrigatório.")]
        [StringLength(80, ErrorMessage = "O Titulo do evento deve conter no maximo 80 caracteres")]
        public string Titulo { get; set; } = null!;

        [Required(ErrorMessage = "A Descrição do evento é obrigatoria.")]
        public string Descricao { get; set; } = null!;

        public IFormFile? Imagem { get; set; }


    }
}
