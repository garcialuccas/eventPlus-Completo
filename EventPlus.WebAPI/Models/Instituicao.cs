using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Models;

[Index("Cnpj", Name = "UQ__Institui__AA57D6B4DBA948FD", IsUnique = true)]
public partial class Instituicao
{
    [Key]
    [Column("IDInstituicao")]
    public Guid Idinstituicao { get; set; }

    [Column("CNPJ")]
    [StringLength(14)]
    [Unicode(false)]
    public string Cnpj { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string NomeFantasia { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string Endereco { get; set; } = null!;

    [InverseProperty("Instituicao")]
    public virtual ICollection<Evento> Evento { get; set; } = new List<Evento>();
}
