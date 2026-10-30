using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces
{
    public interface ITipoUsuario
    {
        Task Cadastrar(TipoUsuario tipoUsuario);

        Task<List<TipoUsuario>> Listar();

        Task Atualizar(Guid id, TipoUsuario tipoUsuario);

        Task Deletar(Guid id);

        Task<TipoUsuario?> BuscarPorId(Guid id);

    }
}
