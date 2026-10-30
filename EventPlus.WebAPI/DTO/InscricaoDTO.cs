using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class InscricaoDTO
    {

        [Required(ErrorMessage = "O usuario é obrigatorio")]
        public Guid? UsuarioId { get; set; }

        [Required(ErrorMessage = "O evento é obrigatorio")]
        public Guid? EventoId { get; set; }

        [Required(ErrorMessage = "A situação é obrigatoria")]
        public bool? Situacao { get; set; }

    }
}
