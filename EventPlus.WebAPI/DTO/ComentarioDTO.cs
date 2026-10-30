using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class ComentarioDTO
    {
        [Required (ErrorMessage = "O Evento é obrigatorio")]
        public Guid? EventoId { get; set; }

        [Required (ErrorMessage = "O Usuario é obrigatorio")]
        public Guid? UsuarioId { get; set; }

        [Required (ErrorMessage = "O Comentario é obrigatorio")]
        public string ComentarioUsuario { get; set; } = null!;

        [Required (ErrorMessage = "O Exibe é obrigatorio")]

        public bool? Exibe { get; set; }

        public DateTime DataComentario { get; set; } = DateTime.Now;

    }
}
