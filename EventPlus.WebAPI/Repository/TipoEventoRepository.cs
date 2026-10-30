using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repository
{
    public class TipoEventoRepository : ITipoEvento
    {
        private readonly EventContext _context;
        public TipoEventoRepository(EventContext context)
        {
            _context = context;
        }

        public async Task Atualizar(Guid id, TipoEvento tipoEvento)
        {
            var TipoEventoEncontrado = await _context.TipoEvento.FindAsync(id);

            if (TipoEventoEncontrado != null)
            {
                TipoEventoEncontrado.Titulo = tipoEvento.Titulo;
                _context.TipoEvento.Update(TipoEventoEncontrado);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<TipoEvento?> BuscarPorId(Guid id)
        {
            return await _context.TipoEvento.FirstOrDefaultAsync(x => x.IdtipoEvento == id);
        }

        public async Task Cadastrar(TipoEvento tipoEvento)
        {
            await _context.TipoEvento.AddAsync(tipoEvento);
            await _context.SaveChangesAsync();

        }

        public async Task Deletar(Guid id)
        {
            var UsuarioEncontrado = await _context.TipoEvento.FindAsync(id);

            if (UsuarioEncontrado != null)
            {
                _context.TipoEvento.Remove(UsuarioEncontrado);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<List<TipoEvento>> Listar()
        {
            return await _context.TipoEvento.AsNoTracking().ToListAsync();
        }
    }
}
