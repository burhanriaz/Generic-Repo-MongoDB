using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Utils;

namespace Web.Domain.Models
{
    public class EmailRequest
    {
        public EmailRequest(List<string> to, string subject, string content, bool isContentHtml = true, AttachmentCollection attachments = null)
        {
            To = to;
            From =  Constants.DefaultFromEmail;
            DisplayName = Constants.DefaultEmailDisplayName;
            Subject = subject;
            Content = content;
            IsContentHtml = isContentHtml;
            Attachments = attachments;
    }

        public List<string> To { get; private set; }
        public string From { get; private set; }
        public string DisplayName { get; set; }
        public string Subject { get; private set; }
        public string Content { get; private set; }
        public bool IsContentHtml { get; private set; }
        public AttachmentCollection Attachments { get; set; }
    }
}
