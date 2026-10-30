using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class InscricaoAtualizarDTO
    {
        public Guid? UsuarioId { get; set; }

        public Guid? EventoId { get; set; }

        public bool? Situacao { get; set; }

    }
}
