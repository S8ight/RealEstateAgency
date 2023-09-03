using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using REA.AuthorizationSystem.BLL.Interfaces;

namespace REA.AuthorizationSystem.BLL.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;
    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    
    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var mailMessage = new MailMessage("bhoof.inc@gmail.com", toEmail);
        mailMessage.Subject = subject;
        mailMessage.Body = message;

        try
        {
            var smptClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("bhoof.inc@gmail.com", 
                    _configuration["Email:Password"]),
                EnableSsl = true
            };

            await smptClient.SendMailAsync(mailMessage);
            _logger.LogInformation("Email sent to: {ToEmail}", toEmail);
        }
        catch (SmtpException e)
        {
            _logger.LogError(e, "Error sending email to {ToEmail}", toEmail);
            throw;
        }
    }
}