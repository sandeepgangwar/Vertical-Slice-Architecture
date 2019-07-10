using System.Linq;
using System.Reflection;
using AutoMapper;

namespace VerticalSliceArchitecture.API.Configurations.Mappings
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                var assembly = typeof(AutoMapperConfig)
                    .GetTypeInfo()
                    .Assembly;

                var mappingProfiles = assembly.GetTypes()
                    .Where(type => typeof(Profile).IsAssignableFrom(type))
                    .ToList();

                cfg.ValidateInlineMaps = false;
                cfg.CreateMissingTypeMaps = true;
                cfg.AddMaps(mappingProfiles);
            });

            return config.CreateMapper();
        }
    }
}
