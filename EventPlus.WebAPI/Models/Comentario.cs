using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Models;

public partial class Comentario
{
    [Key]
    [Column("ComentarioID")]
    public Guid ComentarioId { get; set; }

    [Column("EventoID")]
    public Guid? EventoId { get; set; }

    [Column("UsuarioID")]
    public Guid? UsuarioId { get; set; }

    [Unicode(false)]
    public string ComentarioUsuario { get; set; } = null!;

    public bool? Exibe { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DataComentario { get; set; }

    [ForeignKey("EventoId")]
    [InverseProperty("Comentario")]
    public virtual Evento? Evento { get; set; }

    [ForeignKey("UsuarioId")]
    [InverseProperty("Comentario")]
    public virtual Usuario? Usuario { get; set; }
}
