using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories
{
    public class TipoUsuarioRepository : ITipoUsuario
    {
        private readonly EventContext _context;
        public TipoUsuarioRepository(EventContext context)
        {
            _context = context;
        }

        public async Task Atualizar(Guid id, TipoUsuario tipoUsuario)
        {
            var TipoUsuarioEncontrado = await _context.TipoUsuario.FindAsync(id);

            if (TipoUsuarioEncontrado != null)
            {
                TipoUsuarioEncontrado.Titulo = tipoUsuario.Titulo;
                _context.TipoUsuario.Update(TipoUsuarioEncontrado);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<TipoUsuario?> BuscarPorId(Guid id)
        {
            return await _context.TipoUsuario.FirstOrDefaultAsync(x => x.IdtipoUsuario == id);
        }

        public async Task Cadastrar(TipoUsuario tipoUsuario)
        {
            await _context.TipoUsuario.AddAsync(tipoUsuario);
            await _context.SaveChangesAsync();

        }

        public async Task Deletar(Guid id)
        {
            var UsuarioEncontrado = await _context.TipoUsuario.FindAsync(id);

            if (UsuarioEncontrado != null)
            {
                _context.TipoUsuario.Remove(UsuarioEncontrado);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<List<TipoUsuario>> Listar()
        {
            return await _context.TipoUsuario.AsNoTracking().ToListAsync();
        }
    }
}
