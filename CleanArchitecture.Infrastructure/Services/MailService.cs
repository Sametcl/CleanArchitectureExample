using CleanArchitecture.Application.Services;
using FluentEmail.Core;
using FluentEmail.Smtp;

namespace CleanArchitecture.Infrastructure.Services
{
    public sealed class MailService : IMailService
    {
        private readonly IFluentEmail fluentEmail;

        public MailService(IFluentEmail fluentEmail)
        {
            this.fluentEmail = fluentEmail;
        }

        public async Task SendMailAsync(string to, string name)
        {

            string htmlTemplate = $@"
        <!DOCTYPE html>
        <html lang='tr'>
        <head>
            <meta charset='UTF-8'>
            <style>
                body {{
                    font-family: 'Segoe UI', sans-serif;
                    background-color: #f4f6f8;
                    padding: 20px;
                }}
                .container {{
                    background: #ffffff;
                    border-radius: 10px;
                    padding: 25px;
                    max-width: 600px;
                    margin: 0 auto;
                    box-shadow: 0 2px 10px rgba(0,0,0,0.1);
                }}
                h2 {{ color: #007bff; }}
                p {{ color: #333333; }}
                .footer {{ margin-top: 20px; font-size: 13px; color: #777; }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h2>👋 Hoş Geldin, {name}!</h2>
                <p>Mail sistemimiz başarıyla çalışıyor 🎉</p>
                <p>Artık sistemden bildirim veya onay mailleri gönderebilirsin.</p>
                <div class='footer'>Bu e-posta otomatik gönderilmiştir. Lütfen cevap vermeyin.</div>
            </div>
        </body>
        </html>
        ";

            await fluentEmail
                .To(to)
                .Subject("Hos Geldiniz")
                .Body(htmlTemplate, isHtml:true)
                .SendAsync();
        }
    }
}
