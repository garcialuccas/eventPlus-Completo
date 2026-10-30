using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces
{
    public interface ITipoEvento
    {
        Task Cadastrar(TipoEvento tipoEvento);

        Task<List<TipoEvento>> Listar();

        Task Atualizar(Guid id, TipoEvento tipoEvento);

        Task Deletar(Guid id);

        Task<TipoEvento?> BuscarPorId(Guid id);

    }
}
