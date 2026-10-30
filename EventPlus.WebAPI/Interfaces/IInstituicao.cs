using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces
{
    public interface IInstituicao
    {
        Task Cadastrar(Instituicao instituicao);

        Task <List<Instituicao>> Listar();

        Task Deletar(Guid id);

        Task Atualizar(Guid id, Instituicao instituicao);

        Task<Instituicao?> BuscarPorId(Guid id);
    }
}
