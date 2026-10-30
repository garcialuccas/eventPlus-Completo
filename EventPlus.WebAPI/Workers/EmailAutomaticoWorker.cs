using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Utils;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Workers
{
    public class EmailAutomaticoWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<EmailAutomaticoWorker> _logger;
      

        public EmailAutomaticoWorker(IServiceScopeFactory serviceScopeFactory, ILogger<EmailAutomaticoWorker> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker de envio de e-mails iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var ctx = scope.ServiceProvider.GetRequiredService<EventContext>();
                        var enviarEmail = scope.ServiceProvider.GetRequiredService<IEnviarEmailInterface>();

                        var inicioJanela = DateTime.UtcNow;
                        var fimJanela = DateTime.UtcNow.AddDays(1);

                        var presencas = await ctx.Presenca
                            .Include(x => x.Usuario)
                            .Include(x => x.Evento)
                                .ThenInclude(x => x.IdStatusEventoNavigation)
                            .Where(x => x.Evento.DataEvento >= inicioJanela
                                     && x.Evento.DataEvento <= fimJanela
                                     && x.Evento.IdStatusEventoNavigation.Status == "PENDENTE")
                            .ToListAsync(stoppingToken);

                        _logger.LogInformation("Presenças encontradas: {Count}", presencas.Count);

                       

                        foreach (var presenca in presencas)
                        {
                            if (presenca.Usuario == null || presenca.Evento == null) continue;

                            try
                            {
                                var assunto = $"Lembrete: {presenca.Evento.Titulo} é amanhã!";
                                var corpo = $@"
                                    <h2>Olá, {presenca.Usuario.Nome}!</h2>
                                    <p>Seu evento <strong>{presenca.Evento.Titulo}</strong> está agendado para {presenca.Evento.DataEvento:dd/MM/yyyy HH:mm}.</p>
                                    <p>Não esqueça de comparecer!</p>";

                                await enviarEmail.EnviarEmailAsync(presenca.Usuario.Email, assunto, corpo, stoppingToken);         
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Erro ao enviar e-mail para {Email} na presença {PresencaId}", presenca.Usuario.Email, presenca.PresencaId);
                            }
                        }
                            await ctx.SaveChangesAsync(stoppingToken);
                        }
                    }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Falha geral na execução do Worker de e-mails.");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}