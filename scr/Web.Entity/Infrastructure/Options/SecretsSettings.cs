namespace Web.Entity.Infrastructure.Options
{
    public class SecretsSettings
    {
        // public string PasswordEncryptionKey { get; set; }

        public string JwtRsaPrivateKeyXml { get; set; }

        public string JwtRsaPublicKeyXml { get; set; }

        public string MasterPassword { get; set; }
    }
}
