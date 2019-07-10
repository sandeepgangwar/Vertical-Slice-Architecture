namespace VerticalSliceArchitecture.Infrastructure.Configurations.App
{
    public class RefreshTokenSettings : ISettings
    {
        public int ExpiryMinutes { get; set; }
    }
}
