using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repository
{
    public class ComentarioRepository : IComentario
    {
        private readonly EventContext _eventContext;

        public ComentarioRepository(EventContext eventContext )
        {
            _eventContext = eventContext;
        }

        public async Task<Comentario?> BuscarPorId(Guid id)
        {
            return await _eventContext.Comentario.Include(x => x.Usuario).FirstOrDefaultAsync(x => x.ComentarioId == id);
        }

        public async Task Cadastrar(Comentario comentario)
        {
            await _eventContext.Comentario.AddAsync(comentario);
            await _eventContext.SaveChangesAsync();
        }

        public async Task Deletar(Guid id)
        {
            var ComentarioEncontra = await _eventContext.Comentario.FindAsync(id);

            if(ComentarioEncontra != null) 
            {
                _eventContext.Comentario.Remove(ComentarioEncontra);
                await _eventContext.SaveChangesAsync();
            
            }
        }

        public async Task EditarVisible(Guid id, Comentario comentario)
        {
            var ComentarioEncontrado = await _eventContext.Comentario.FindAsync(id);

            if (ComentarioEncontrado != null) 
            {
                ComentarioEncontrado.Exibe = comentario.Exibe == null ? ComentarioEncontrado.Exibe : comentario.Exibe;

                _eventContext.Comentario.Update(ComentarioEncontrado);

                await _eventContext.SaveChangesAsync();
            }
        }

        public async Task<List<Comentario>> Listar()
        {
            return await _eventContext.Comentario.Where(x => x.Exibe == true).AsNoTracking().ToListAsync();
        }

        public async Task<List<Comentario>> ListarOcultos()
        {
            return await _eventContext.Comentario.Where(x => x.Exibe == false).AsNoTracking().ToListAsync();
        }

        public async Task<List<Comentario>> ListarPorEvento(Guid idevento)
        {
            return await _eventContext.Comentario.Include(x => x.Usuario).Where(x => x.EventoId == idevento).AsNoTracking().ToListAsync();
        }
    }
}
