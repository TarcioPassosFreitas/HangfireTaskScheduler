using Ardalis.Result;

namespace HangfireTaskScheduler.Core.Interfaces.Service;

public interface IEmailSenderService
{
    Task<Result> SendEmailAsync(string fromEmail, string fromName, string toEmail, string subject, string message, string templateId);
}