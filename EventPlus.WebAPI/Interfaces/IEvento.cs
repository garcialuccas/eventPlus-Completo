using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces
{
    public interface IEvento
    {
        Task Cadastro(Evento evento);
        Task<List<Evento>> Listar();
        Task Deletar(Guid id);
        Task Atualizar(Guid id, Evento evento);
        Task<Evento?> BuscarPorId(Guid id);
        Task<List<Evento>> ListarPoInstituicao(Guid id);
        Task<List<Evento>> ListarPorInscrito(Guid id);
        Task<List<Evento>> ListarProximos();
    }
}
