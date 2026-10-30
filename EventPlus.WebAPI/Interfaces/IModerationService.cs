namespace EventPlus.WebAPI.Interfaces
{
    public interface IModerationService
    {
        Task<bool> moderarTexto(string texto);
    }
}
