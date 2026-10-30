using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repository
{
    public class InscricaoRepository : IInscricao
    {
        private readonly EventContext _eventContext;

        public InscricaoRepository(EventContext eventContext)
        {
            _eventContext = eventContext;
        }

        public async Task AtualizarSituacao(Guid id, Presenca presenca)
        {
            var PresencaEncontrada = await _eventContext.Presenca.FindAsync(id);

            if (PresencaEncontrada != null) 
            {
                PresencaEncontrada.UsuarioId = presenca.UsuarioId == null ? PresencaEncontrada.UsuarioId : presenca.UsuarioId;
                PresencaEncontrada.EventoId = presenca.EventoId == null ? PresencaEncontrada.EventoId : presenca.EventoId;
                PresencaEncontrada.Situacao = presenca.Situacao == null ? PresencaEncontrada.Situacao : presenca.Situacao;

                _eventContext.Presenca.Update(PresencaEncontrada);
                await _eventContext.SaveChangesAsync();
            }

        }

        public async Task<Presenca?> BuscarPorId(Guid id)
        {
            return await _eventContext.Presenca.FirstOrDefaultAsync(x => x.PresencaId == id);
        }

        public async Task Cadastrar(Presenca presenca)
        {
            await _eventContext.Presenca.AddAsync(presenca);
            await _eventContext.SaveChangesAsync();
        }

        public async Task Deletar(Guid id)
        {
            var PresencaEncontrada = await _eventContext.Presenca.FindAsync(id);

            if (PresencaEncontrada != null) 
            {
                _eventContext.Remove(PresencaEncontrada);
                await _eventContext.SaveChangesAsync();
            }
        }

        public async Task<List<Presenca>> Listar()
        {
            return await _eventContext.Presenca.Include(x => x.Evento).Include(x => x.Usuario).AsNoTracking().ToListAsync();
        }

        public async Task<List<Presenca>> ListarMinhasPresencas(Guid id)
        {
            return await _eventContext.Presenca.Include(x => x.Usuario).Include(x => x.Evento).Where(x => x.UsuarioId == id).ToListAsync();
        }
    }
}
