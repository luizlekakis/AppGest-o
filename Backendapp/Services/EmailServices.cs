using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AppGestorResumo.Services
{
    public class EmailService
    {
        public async Task EnviarEmailAsync(string destino, string assunto, string corpo)
        {
            var mensagem = new MimeMessage();
            mensagem.From.Add(MailboxAddress.Parse("seuemail@empresa.com"));
            mensagem.To.Add(MailboxAddress.Parse(destino));
            mensagem.Subject = assunto;
            mensagem.Body = new TextPart("plain") { Text = corpo };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.seudominio.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync("seuemail@empresa.com", "SUA_SENHA");
            await client.SendAsync(mensagem);
            await client.DisconnectAsync(true);
        }
    }
}
