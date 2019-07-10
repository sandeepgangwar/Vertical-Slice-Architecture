using System;
using Microsoft.Extensions.Configuration;

namespace VerticalSliceArchitecture.Infrastructure.Extensions
{
    public static class SettingsExtensions
    {
        public static object GetSettings(this IConfiguration configuration, Type type)
        {
            var section = type.Name.Replace("Settings", string.Empty);
            var configurationValue = Activator.CreateInstance(type);
            configuration.GetSection(section).Bind(configurationValue);

            return configurationValue;
        }
    }
}
