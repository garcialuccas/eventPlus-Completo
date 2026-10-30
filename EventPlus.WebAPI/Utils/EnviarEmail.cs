using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Workers;
using MimeKit;
using MailKit.Net.Smtp;


namespace EventPlus.WebAPI.Utils
{
    public class EnviarEmail : IEnviarEmailInterface
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailAutomaticoWorker> _logger;


        public EnviarEmail(IConfiguration configuration, ILogger<EmailAutomaticoWorker> logger)
        {
            _config = configuration;
            _logger = logger;
        }

        public async Task EnviarEmailAsync(string destinatario, string assunto, string corpo, CancellationToken c = default)
        {
            var mensagem = new MimeMessage();
            mensagem.From.Add(MailboxAddress.Parse(_config["Smtp:RemetenteEmail"]));
            mensagem.To.Add(MailboxAddress.Parse(destinatario));
            mensagem.Subject = assunto;
            mensagem.Body = new TextPart("html") { Text = corpo };

            using var client = new SmtpClient();

            try
            {
                await client.ConnectAsync(_config["Smtp:Host"]!,
                    int.Parse(_config["Smtp:Port"]!),
                    MailKit.Security.SecureSocketOptions.StartTls, c);

                await client.AuthenticateAsync(_config["Smtp:Usuario"]!, _config["Smtp:Senha"]!, c);

                await client.SendAsync(mensagem, c);
                await client.DisconnectAsync(true, c);

                _logger.LogInformation($"Email enviado para {destinatario}, sobre {assunto}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao enviar email para {destinatario}");
            }


        }
    }
}
