using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using MovieApp.Domain.Abstractions;

namespace MovieApp.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var username = _configuration.GetValue<string>("EmailConfig:Username");
        var password = _configuration.GetValue<string>("EmailConfig:Password");
        var host = _configuration.GetValue<string>("EmailConfig:Host");
        var port = _configuration.GetValue<int>("EmailConfig:Port");
        
        var smtpClient = new SmtpClient(host, port);
        smtpClient.EnableSsl = true;
        smtpClient.UseDefaultCredentials = false;
        
        smtpClient.Credentials = new NetworkCredential(username, password);
        
        var mailMessage = new MailMessage
        {
            From = new MailAddress(username, "MovieApp"),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        
        mailMessage.To.Add(toEmail);
        await smtpClient.SendMailAsync(mailMessage);
    }
}