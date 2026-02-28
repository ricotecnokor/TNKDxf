using Microsoft.Extensions.DependencyInjection;
using TNKDxf.TeklaManipulacao.Adapters;

namespace TNKDxf
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddConverteDxfs(this IServiceCollection services)
        {
            services.AddTransient<IAdapterDesenho, AdapterDesenho>();

            return services;
        }
    }
}
