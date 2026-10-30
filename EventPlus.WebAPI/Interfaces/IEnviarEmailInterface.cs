namespace EventPlus.WebAPI.Interfaces
{
    public interface IEnviarEmailInterface
    {
        Task EnviarEmailAsync(string destinatario, string assunto, string corpo, CancellationToken c = default);
    }
}
