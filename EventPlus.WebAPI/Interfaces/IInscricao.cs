using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces
{
    public interface IInscricao
    {
        public Task Cadastrar(Presenca presenca);

        public Task AtualizarSituacao(Guid id, Presenca presenca);

        public Task Deletar(Guid id);

        public Task<List<Presenca>> Listar();

        public Task<List<Presenca>> ListarMinhasPresencas(Guid id);
        public Task<Presenca> BuscarPorId(Guid id);

    }
}
