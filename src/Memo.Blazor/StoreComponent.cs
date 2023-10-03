using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Memo.Blazor;

public abstract class StoreComponent : ComponentBase, IStoreObserver
{
    [Inject]
    IServiceProvider ServiceProvider { get; set; } = null!;

    private readonly Dictionary<Type, IStore> _attachedStores = new();

    void IStoreObserver.StateChanged(IStore store) => StateHasChanged();

    protected T AttachStore<T>() where T : IStore
    {
        if (!_attachedStores.TryGetValue(typeof(T), out var store))
        {
            _attachedStores[typeof(T)] = store = ServiceProvider.GetRequiredService<T>();
            store.Listen(this);
        }

        return (T)store;
    }
}

