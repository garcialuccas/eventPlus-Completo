using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class InstituicaoDTO
    {

        [Required(ErrorMessage = "O CNPJ é obrigatorio")]
        [StringLength(14, ErrorMessage = "O CNPJ pode conter no máximo 14 caracteres.")]
        public string CNPJ { get; set; } = string.Empty;


        [Required(ErrorMessage = "O Nome Fantasia é obrigatória.")]
        [StringLength(100, ErrorMessage = "A Nome fantasia pode conter no máximo 100 caracteres.")]
        public string NomeFantasia { get; set; } = string.Empty;


        [Required(ErrorMessage = "O Endereco é obrigatorio")]
        [StringLength(150, ErrorMessage = "O Endereco pode conter no maximo 150 caracteres")]

        public string Endereco { get; set; }



    }
}
