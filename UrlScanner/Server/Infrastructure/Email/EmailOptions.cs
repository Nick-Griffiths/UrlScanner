namespace UrlScanner.Server.Infrastructure.Email
{
    internal sealed class EmailOptions
    {
        public bool SendingIsEnabled { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public SmtpOptions Smtp { get; set; }
    }
}