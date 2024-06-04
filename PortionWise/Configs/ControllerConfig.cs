using System.Text.Json.Serialization;

namespace PortionWise.Configs
{
    class ControllerConfig
    {
        public static void config(WebApplicationBuilder builder)
        {
            builder
                .Services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                    options.JsonSerializerOptions.DefaultIgnoreCondition =
                        JsonIgnoreCondition.WhenWritingNull;
                });
        }
    }
}
