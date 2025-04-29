using Azure;
using Azure.Communication.Email;
using GoodExpense.Notification.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GoodExpense.Notification.Application;

public class AzureEmailNotificationService : INotificationService
{
    private readonly AzureNotificationConfiguration _azureNotificationConfiguration;
    private readonly ILogger<AzureEmailNotificationService> _logger;

    public AzureEmailNotificationService(
        IOptions<AzureNotificationConfiguration> azureNotificationOptions,
        ILogger<AzureEmailNotificationService> logger)
    {
        _azureNotificationConfiguration = azureNotificationOptions.Value;
        _logger = logger;
    }

    public async Task SendNotificationAsync(NotifyRequest request)
    {
        var client = new EmailClient(_azureNotificationConfiguration.ConnectionString);
        var content = new EmailContent(request.Subject)
        {
            Html = request.Body,
        };
        var recipients = new EmailRecipients(request.Recipients.Select(r => new EmailAddress(r)).ToList());
        var message = new EmailMessage(_azureNotificationConfiguration.FromAddress, recipients, content);
        
        foreach(var attachment in request.Attachments)
        {
            var emailAttachment = new EmailAttachment(
                attachment.FileName,
                attachment.ContentType,
                BinaryData.FromBytes(Convert.FromBase64String(attachment.ContentBase64)));
            message.Attachments.Add(emailAttachment);
        }

        try
        {
            var sendResult = await client.SendAsync(WaitUntil.Completed, message);
            if (sendResult.HasCompleted)
            {
                _logger.LogInformation("Email sent successfully to {ToAddress}.", request.Recipients);
            }
            else
            {
                _logger.LogError("Email sending failed to : {Recipient}", request.Recipients);
            }
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError("Email sending error: {ExMessage}", ex.Message);
        }
    }
}