using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Memo;

public static class ServiceCollectionExtensions
{
    public static void AddMemoStoresFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (typeof(ILocalStore).IsAssignableFrom(type))
            {
                services.AddTransient(type)
                    .AddScoped(p => (IStore)p.GetRequiredService(type));
            }
            else if (typeof(IGlobalStore).IsAssignableFrom(type))
            {
                services.AddSingleton(type)
                    .AddScoped(p => (IStore)p.GetRequiredService(type));
            }
            else if (typeof(IStore).IsAssignableFrom(type))
            {
                throw new InvalidOperationException($"Store {type} must be either Local (derives from LocalStore) or Global (derives from GlobalStore)");
            }
        }
    }
}
