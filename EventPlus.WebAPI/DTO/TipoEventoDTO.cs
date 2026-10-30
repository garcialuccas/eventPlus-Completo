using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class TipoEventoDTO
    {

        [Required(ErrorMessage = "O Titulo é obrigatório.")]
        [StringLength(100, ErrorMessage = "O titulo pode ter no máximo 100 caracteres.")]
        public string Titulo { get; set; } = string.Empty;
    }
}
