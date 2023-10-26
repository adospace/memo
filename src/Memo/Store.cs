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

    protected Store(Func<T> initialStateFunc)
    {
        State = initialStateFunc();
    }

    protected void Mutate(Func<T, T> stateFunc)
    {
        State = stateFunc(State);

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
    protected LocalStore(Func<T> initialStateFunc):base(initialStateFunc) { }
}

public abstract class GlobalStore<T> : Store<T>, IGlobalStore where T : class
{
    protected GlobalStore(Func<T> initialStateFunc) : base(initialStateFunc) { }
}

public interface IStoreObserver
{
    void StateChanged(IStore store);
}

