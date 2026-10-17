using Application.Common.Models;
using MimeKit;

namespace Infrastructure.Services;

public static class MimeMessageFactory
{
    public static MimeMessage Create(EmailMessage message)
    {
        var mimeMessage = new MimeMessage();

        mimeMessage.From.Add(new MailboxAddress(message.FromName, message.FromAddress));

        foreach (var to in message.To)
        {
            mimeMessage.To.Add(new MailboxAddress(to, to));
        }

        foreach (var cc in message.Cc)
        {
            mimeMessage.Cc.Add(new MailboxAddress(cc, cc));
        }

        foreach (var bcc in message.Bcc)
        {
            mimeMessage.Bcc.Add(new MailboxAddress(bcc, bcc));
        }

        if (!string.IsNullOrWhiteSpace(message.ReplyToAddress))
        {
            mimeMessage.ReplyTo.Add(new MailboxAddress(message.ReplyToName ?? message.ReplyToAddress,
                message.ReplyToAddress));
        }

        mimeMessage.Subject = message.Subject;
        mimeMessage.Body = new TextPart(message.IsHtml ? "html" : "plain")
        {
            Text = message.Body
        };

        return mimeMessage;
    }
}
