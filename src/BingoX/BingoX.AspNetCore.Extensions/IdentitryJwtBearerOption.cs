namespace Microsoft.Extensions.DependencyInjection
{
    public class IdentitryJwtBearerOption
    {
        public string RSAPublicKey { get; set; }
        public string Authority { get; set; }
        public string Audience { get; set; }
    }
}
