using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces
{
    public interface IUsuario
    {
        Task Cadastrar(Usuario usuarioCadastro);
        Task<List<Usuario>> Listar();
        Task Deletar(Guid id);

        Task Atualizar(Guid id, Usuario usuarioAtualizar);

        Task<Usuario> BuscaPorId(Guid id);

        Task<Usuario?> BuscaPorEmaileSenha(string Email);

        Task NovaPresenca(Guid idEvento, Guid idUsuario);
    }
}
