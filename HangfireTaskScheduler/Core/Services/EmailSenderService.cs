using Ardalis.Result;
using HangfireTaskScheduler.Core.Configuration;
using HangfireTaskScheduler.Core.Interfaces.Service;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace HangfireTaskScheduler.Core.Services;

public class EmailSenderService : IEmailSenderService
{
    private readonly IOptions<MailJetConfiguration> _mailJetConfig;

    public EmailSenderService(IOptions<MailJetConfiguration> mailJetConfig)
    {
        _mailJetConfig = mailJetConfig;
    }

    public async Task<Result> SendEmailAsync(string fromEmail, string fromName, string toEmail, string subject, string message, string templateId)
    {
        MailjetClient client = new MailjetClient(_mailJetConfig.Value.ApiPublic, _mailJetConfig.Value.ApiPrivate);

        MailjetRequest request = new MailjetRequest
        {
            Resource = Send.Resource,
        }
        .Property(Send.Messages, new JArray {
            new JObject {
                {"From", new JObject {
                    {"Email", fromEmail},
                    {"Name", fromName}
                }},
                {"To", new JArray {
                    new JObject {
                        {"Email", toEmail},
                        {"Name", "Recipient's Name"}
                    }
                }},
                {"Subject", subject},
                {"TextPart", message},
                {"HTMLPart", "<h3>"+message+"</h3>"},
                {"TemplateID", templateId},
                {"TemplateLanguage", true}
            }
        });

        MailjetResponse response = await client.PostAsync(request);

        if (response.IsSuccessStatusCode)
        {
            return Result.Success();
        }
        else
        {
            return Result.Error(response.GetErrorMessage());
        }
    }
}