using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_ASP_NET.Utilities
{
    public class MailService : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailJetSettings settings = _configuration.GetSection("MailJet").Get<MailJetSettings>();

            MailjetClient client = new MailjetClient(settings.ApiKey, settings.ApiPrivate);
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
               .Property(Send.FromEmail, "victor.pavlovskyy@gmail.com")
               .Property(Send.FromName, "REPLICA")
               .Property(Send.Subject, subject)
               .Property(Send.TextPart, "Dear passenger, welcome to Mailjet! May the delivery force be with you!")
               .Property(Send.HtmlPart, htmlMessage)
               .Property(Send.Recipients, new JArray {
            new JObject {
             {"Email", email}
             }
                });
            MailjetResponse response = await client.PostAsync(request);
        }
    }
}
