namespace VerticalSliceArchitecture.Infrastructure.Configurations.App
{
    public class JwtSettings : ISettings
    {
        public string Issuer { get; set; }
        public string Key { get; set; }
        public int ExpiryMinutes { get; set; }
    }
}
