using NotificationService.Models; // ya da senin kullandığın klasör ismine göre


namespace NotificationService.Models
{
    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
