using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EventPlus.WebAPI.Repository
{
    public class EventoRepository : IEvento
    {
        private readonly EventContext _context;

        public EventoRepository(EventContext eventContext)
        {
            _context = eventContext;
        }


        public async Task Atualizar(Guid id, Evento evento)
        {
            var EventoEncontrado = await _context.Evento.FindAsync(id);

            if (EventoEncontrado != null) 
            {
                EventoEncontrado.TipoEventoId = evento.TipoEventoId == null ? EventoEncontrado.TipoEventoId : evento.TipoEventoId;
                EventoEncontrado.InstituicaoId = evento.InstituicaoId == null ? EventoEncontrado.InstituicaoId : evento.InstituicaoId;
                EventoEncontrado.DataEvento = evento.DataEvento == DateTime.MinValue ? EventoEncontrado.DataEvento : evento.DataEvento;
                EventoEncontrado.Titulo = string.IsNullOrEmpty(evento.Titulo) ? EventoEncontrado.Titulo : evento.Titulo;
                EventoEncontrado.Descricao = string.IsNullOrEmpty(evento.Descricao) ? EventoEncontrado.Descricao : evento.Descricao;
                EventoEncontrado.ImagemUrl = string.IsNullOrEmpty(evento.ImagemUrl)? EventoEncontrado.ImagemUrl : evento.ImagemUrl;

                 _context.Evento.Update(EventoEncontrado);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Evento?> BuscarPorId(Guid id)
        {
            return await _context.Evento.FirstOrDefaultAsync(x => x.EventoId == id);
        }

        public async Task Cadastro(Evento evento)
        {
            await _context.Evento.AddAsync(evento);
            await _context.SaveChangesAsync();
        }

        public async Task Deletar(Guid id)
        {
            var eventoEncontrado = await _context.Evento.FindAsync(id);

            if (eventoEncontrado != null) 
            {
                _context.Evento.Remove(eventoEncontrado);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Evento>> Listar()
        {
            return await _context.Evento.Include(x => x.Instituicao).Include(x => x.TipoEvento).AsNoTracking().ToListAsync();
        }

        public async Task<List<Evento>> ListarPoInstituicao(Guid id)
        {
            return await _context.Evento.Where(x => x.InstituicaoId == id).AsNoTracking().ToListAsync();
        }

        public async Task<List<Evento>> ListarPorInscrito(Guid id)
        {
            return await _context.Evento.Where(x => x.Presenca.Any(x => x.UsuarioId == id)).AsNoTracking().ToListAsync();
        }

        public async Task<List<Evento>> ListarProximos()
        {
            return await _context.Evento.Where(x => x.DataEvento >= DateTime.Now).OrderByDescending(x => x.DataEvento).AsNoTracking().ToListAsync();
        }
    }
}
