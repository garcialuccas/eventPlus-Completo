using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class UsuarioDTO
    {
        [Required(ErrorMessage = "O Email é obrigatorio")]
        [StringLength(100, ErrorMessage = "O Email pode conter no máximo 100 caracteres.")]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = "A Senha é obrigatória.")]
        [StringLength(100, ErrorMessage = "A Senha pode conter no máximo 60 caracteres.")]
        public string Senha { get; set; } = string.Empty;


        [Required(ErrorMessage = "O Nome é obrigatorio")]
        [StringLength(100, ErrorMessage = "O Nome pode conter no maximo 100 caracteres")]

        public string Nome { get; set; }

        [Required(ErrorMessage = "O Tipo do Usuario é obrigatorio")]

        public Guid TipoUsuarioid { get; set; }

    }
}
