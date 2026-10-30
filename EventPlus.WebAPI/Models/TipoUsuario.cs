using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Models;

public partial class TipoUsuario
{
    [Key]
    [Column("IDTipoUsuario")]
    public Guid IdtipoUsuario { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Titulo { get; set; }

    [InverseProperty("TipoUsuario")]
    public virtual ICollection<Usuario> Usuario { get; set; } = new List<Usuario>();
}
