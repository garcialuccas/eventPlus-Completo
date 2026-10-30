using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces
{
    public interface IComentario
    {
        public Task<List<Comentario>> Listar();
        public Task<List<Comentario>> ListarOcultos();

        public Task Cadastrar(Comentario comentario);

        public Task<List<Comentario>> ListarPorEvento(Guid idevento);

        public Task Deletar(Guid id);

        public Task<Comentario?> BuscarPorId(Guid id);

        public Task EditarVisible(Guid id, Comentario comentario);


    }
}
