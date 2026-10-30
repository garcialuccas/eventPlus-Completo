using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Models;

[Index("Email", Name = "UQ__Usuario__A9D1053494C54D3C", IsUnique = true)]
public partial class Usuario
{
    [Key]
    [Column("UsuarioID")]
    public Guid UsuarioId { get; set; }

    [Column("TipoUsuarioID")]
    public Guid? TipoUsuarioId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(60)]
    [Unicode(false)]
    public string Senha { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [InverseProperty("Usuario")]
    public virtual ICollection<Comentario> Comentario { get; set; } = new List<Comentario>();

    [InverseProperty("Usuario")]
    public virtual ICollection<Presenca> Presenca { get; set; } = new List<Presenca>();

    [ForeignKey("TipoUsuarioId")]
    [InverseProperty("Usuario")]
    public virtual TipoUsuario? TipoUsuario { get; set; }
}
