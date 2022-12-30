using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using TestApp.Data;
using TestApp.Models.Dtos;
using TestApp.Models.Entity;

namespace TestApp.Services
{
    public class CheckIin : ICheckIin
    {
        private readonly IConfiguration _configuration;
        private readonly MyAppContext _context;

        public CheckIin(IConfiguration configuration, MyAppContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task SearchIin(ClientVm clientVm)
        {
            var apiUrl = _configuration["AFM"];
            var client = new HttpClient();
            var response = client.GetAsync(apiUrl).Result;
            if (!response.IsSuccessStatusCode) return;

            XDocument? xdoc = XDocument.Parse(await response.Content.ReadAsStringAsync());
            if (!xdoc.Descendants("iin").Any(n => n.Value == clientVm.Iin))
            {
                await SendEmailAsync(clientVm.Email, "Уведомление",
                    $"Ваш ИИН {clientVm.Iin}. Проведение сделок доступно");
                clientVm.IsAvailable = true;
                _context.Add(new ClientInfo() {Email = clientVm.Email, Iin = clientVm.Iin});
                await _context.SaveChangesAsync();
                return;
            }

            clientVm.IsAvailable = false;
        }
        
        private async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта",
                "bashyrali@mail.ru")); // Здесь указываем ящик с которого будет происходить отправка
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 25, false);
                await client.AuthenticateAsync("bashyrali@mail.ru",
                    "deJhta8TNdG10DpSmZyL"); //Данные для аутентификации ящика с которого отправляется письмо
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}