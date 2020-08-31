namespace Microsoft.Extensions.DependencyInjection
{
    public class IdentitryOpenIdOption
    {
        public string Authority { get; set; }
        public string ResponseType { get; set; }
        public string WebSiteClientId { get; set; }
        public string[] Scopes { get; set; }
        public string ClientSecret { get; set; }
    }
}
