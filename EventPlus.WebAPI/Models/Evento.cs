using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Models;

public partial class Evento
{
    [Key]
    [Column("EventoID")]
    public Guid EventoId { get; set; }

    [Column("TipoEventoID")]
    public Guid? TipoEventoId { get; set; }

    [Column("InstituicaoID")]
    public Guid? InstituicaoId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DataEvento { get; set; }

    [StringLength(80)]
    [Unicode(false)]
    public string Titulo { get; set; } = null!;

    [Column(TypeName = "text")]
    public string Descricao { get; set; } = null!;

    [Column("ImagemURL")]
    [Unicode(false)]
    public string? ImagemUrl { get; set; }

    [Column("idStatusEvento")]
    public Guid? IdStatusEvento { get; set; }

    [InverseProperty("Evento")]
    public virtual ICollection<Comentario> Comentario { get; set; } = new List<Comentario>();

    [ForeignKey("IdStatusEvento")]
    [InverseProperty("Evento")]
    public virtual StatusEvento? IdStatusEventoNavigation { get; set; }

    [ForeignKey("InstituicaoId")]
    [InverseProperty("Evento")]
    public virtual Instituicao? Instituicao { get; set; }

    [InverseProperty("Evento")]
    public virtual ICollection<Presenca> Presenca { get; set; } = new List<Presenca>();

    [ForeignKey("TipoEventoId")]
    [InverseProperty("Evento")]
    public virtual TipoEvento? TipoEvento { get; set; }
}
