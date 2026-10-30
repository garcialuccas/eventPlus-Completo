using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Models;

public partial class TipoEvento
{
    [Key]
    [Column("IDTipoEvento")]
    public Guid IdtipoEvento { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Titulo { get; set; } = null!;

    [InverseProperty("TipoEvento")]
    public virtual ICollection<Evento> Evento { get; set; } = new List<Evento>();
}
