namespace GraphAPIMailSender.Models
{
    public class GraphAPIKeys
    {
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string FromEmail { get; set; }
        public string HtmlPath { get; set; }
    }
}
