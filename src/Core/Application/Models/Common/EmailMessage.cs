namespace Application.Models.Common
{
    public class EmailMessage
    {
        public IDictionary<string, string> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool IsHtml { get; set; }

        public EmailMessage(IDictionary<string, string> to, string subject, string content, bool isHtml = true)
        {
            To = to;
            Subject = subject;
            Content = content;
            IsHtml = isHtml;
        }
    }
}
