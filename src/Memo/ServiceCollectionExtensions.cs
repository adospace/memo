using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
                var alreadyRegisteredService = services.FirstOrDefault(_=>_.ServiceType == type);
                if (alreadyRegisteredService != null &&
                    alreadyRegisteredService.Lifetime != ServiceLifetime.Transient)
                {
                    throw new InvalidOperationException($"Type {type} is already registered with Lifetime {alreadyRegisteredService.Lifetime}: expecting {ServiceLifetime.Transient} as it's a local store");
                }
                else if (alreadyRegisteredService == null)
                {
                    services.AddTransient(type);
                }
            }
            else if (typeof(IGlobalStore).IsAssignableFrom(type))
            {
                var alreadyRegisteredService = services.FirstOrDefault(_ => _.ServiceType == type);
                if (alreadyRegisteredService != null &&
                    alreadyRegisteredService.Lifetime != ServiceLifetime.Singleton)
                {
                    throw new InvalidOperationException($"Type {type} is already registered with Lifetime {alreadyRegisteredService.Lifetime}: expecting {ServiceLifetime.Singleton} as it's a global store");
                }
                else if (alreadyRegisteredService == null)
                {
                    services.AddSingleton(type);
                }
            }
            else if (typeof(IStore).IsAssignableFrom(type))
            {
                throw new InvalidOperationException($"Store {type} must be either Local (derives from LocalStore) or Global (derives from GlobalStore)");
            }
        }
    }
}
