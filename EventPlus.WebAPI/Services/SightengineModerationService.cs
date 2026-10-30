using EventPlus.WebAPI.Interfaces;

namespace EventPlus.WebAPI.Services
{
    public class SightengineModerationService : IModerationService
    {
        private readonly IModerationService _moderationService;
        public SightengineModerationService(IModerationService moderationService)
        {
            _moderationService = moderationService;
        }


        public Task<bool> moderarTexto(string texto)
        {
            throw new NotImplementedException();
        }
    }
}
