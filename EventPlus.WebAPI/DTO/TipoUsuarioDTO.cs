using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO;
/// <summary>
/// Data Tranfer Object (DTO) para cadastro e atualização do Perfil/Tipo de Usuário.
/// </summary>
public class TipoUsuarioDTO
{
    /// <summary>
    /// Titulo do tipo de usuário
    /// </summary>

    [Required(ErrorMessage = "O Titulo é obrigatório.")]
    [StringLength(100, ErrorMessage = "O titulo pode ter no máximo 100 caracteres.")]
    public string Titulo { get; set; } = string.Empty;
}
