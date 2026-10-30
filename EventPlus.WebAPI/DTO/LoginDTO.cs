using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class LoginDTO
    {

        [Required(ErrorMessage = "O Email é obrigatorio")]
        [EmailAddress(ErrorMessage = "Informe um email valido")]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = "A Senha é obrigatória.")]
        [StringLength(60, ErrorMessage = "A Senha pode conter no máximo 60 caracteres e no minimo 8 caracteres.")]
        public string Senha { get; set; } = string.Empty;

    }
}
