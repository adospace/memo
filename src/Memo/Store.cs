namespace Memo;

public interface IStore
{
    void Listen(IStoreObserver observer);
}

public interface ILocalStore : IStore
{

}

public interface IGlobalStore : IStore
{

}

public abstract class Store<T> : IStore where T : class
{
    private readonly List<WeakReference<IStoreObserver>> _listeners = new();

    public T State { get; private set; }

    protected Store()
    {
        State = InitialState();
    }

    protected abstract T InitialState();

    protected void Mutate(T state)
    {
        State = state;

        for (int i = 0; i < _listeners.Count; i++)
        {
            if (_listeners[i].TryGetTarget(out var listener))
            {
                listener.StateChanged(this);
            }
            else
            {
                _listeners.RemoveAt(i);
                i--;
            }
        }
    }

    void IStore.Listen(IStoreObserver observer)
    {
        _listeners.Add(new WeakReference<IStoreObserver>(observer));
    }
}

public abstract class LocalStore<T> : Store<T>, ILocalStore where T : class
{

}

public abstract class GlobalStore<T> : Store<T>, IGlobalStore where T : class
{

}

public interface IStoreObserver
{
    void StateChanged(IStore store);
}

