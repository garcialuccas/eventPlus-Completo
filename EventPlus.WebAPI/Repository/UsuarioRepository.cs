using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.Intrinsics.Arm;

namespace EventPlus.WebAPI.Repository
{
    public class UsuarioRepository : IUsuario
    {
        private readonly EventContext _context;

        public UsuarioRepository(EventContext Context)
        {
            _context = Context;
        }


        public async Task Atualizar(Guid id, Usuario usuarioAtualizar)
        {
            var usuarioEncontrado = await _context.Usuario.FindAsync(id);

            if (usuarioEncontrado != null)
            {
                usuarioEncontrado.Email = string.IsNullOrEmpty(usuarioAtualizar.Email) ? usuarioEncontrado.Email : usuarioAtualizar.Email;
                usuarioEncontrado.Senha = string.IsNullOrEmpty(usuarioAtualizar.Senha) ? usuarioEncontrado.Senha : usuarioAtualizar.Senha;
                usuarioEncontrado.Nome =  string.IsNullOrEmpty(usuarioAtualizar.Nome) ? usuarioEncontrado.Nome : usuarioAtualizar.Nome;
                usuarioEncontrado.TipoUsuarioId = usuarioAtualizar.TipoUsuarioId == null ? usuarioEncontrado.TipoUsuarioId : usuarioAtualizar.TipoUsuarioId;

                _context.Usuario.Update(usuarioEncontrado);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Usuario?> BuscaPorEmaileSenha(string email)
        {
            return await _context.Usuario.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Usuario> BuscaPorId(Guid id)
        {
            return await _context.Usuario.FirstOrDefaultAsync(x => x.UsuarioId == id);
        }

        public async Task Cadastrar(Usuario usuarioCadastro)
        {
            await _context.Usuario.AddAsync(usuarioCadastro);
            await _context.SaveChangesAsync();
        }

        public async Task Deletar(Guid id)
        {
            var UsuarioEncontrado = await _context.Usuario.FindAsync(id);

            if (UsuarioEncontrado != null)
            {
                _context.Usuario.Remove(UsuarioEncontrado);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Usuario>> Listar()
        {
            return await _context.Usuario.Include(x => x.TipoUsuario).AsNoTracking().ToListAsync();
        }

        public async Task NovaPresenca(Guid idEvento, Guid idUsuario)
        {
                Presenca usuarioPresenca = new Presenca
                {
                    UsuarioId = idUsuario,
                    EventoId = idEvento,
                    Situacao = true
                };

                _context.Presenca.AddAsync(usuarioPresenca);
                await _context.SaveChangesAsync();
            }
    }
}
