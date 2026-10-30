using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class UsuarioAtualizarDTO
    {

        [StringLength(100, ErrorMessage = "O Email pode conter no máximo 100 caracteres.")]
        public string Email { get; set; } = string.Empty;


        [StringLength(100, ErrorMessage = "A Senha pode conter no máximo 60 caracteres.")]
        public string Senha { get; set; } = string.Empty;


        [StringLength(100, ErrorMessage = "O Nome pode conter no maximo 100 caracteres")]

        public string Nome { get; set; } = string.Empty;

        public Guid? TipoUsuarioid { get; set; }

    }
}
