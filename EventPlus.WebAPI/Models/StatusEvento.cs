using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Models;

public partial class StatusEvento
{
    [Key]
    [Column("idStatusEvento")]
    public Guid IdStatusEvento { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [InverseProperty("IdStatusEventoNavigation")]
    public virtual ICollection<Evento> Evento { get; set; } = new List<Evento>();
}
