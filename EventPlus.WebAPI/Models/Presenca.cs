using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Models;

public partial class Presenca
{
    [Key]
    [Column("PresencaID")]
    public Guid PresencaId { get; set; }

    [Column("UsuarioID")]
    public Guid? UsuarioId { get; set; }

    [Column("EventoID")]
    public Guid? EventoId { get; set; }

    public bool? Situacao { get; set; }

    [ForeignKey("EventoId")]
    [InverseProperty("Presenca")]
    public virtual Evento? Evento { get; set; }

    [ForeignKey("UsuarioId")]
    [InverseProperty("Presenca")]
    public virtual Usuario? Usuario { get; set; }
}
