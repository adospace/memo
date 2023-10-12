namespace Memo.Tests;

public class TestStoreObserver
{
    record CounterState(int Counter);

    class CounterStore : LocalStore<CounterState>
    {
        public CounterStore(): base(()=>new(0)) { }

        public void Increment()
            => Mutate(s => s with { Counter = State.Counter + 1 });
    }

    class CounterStateObserver : IStoreObserver
    {
        public CounterState? LastStateObserved { get; set; }
        public void StateChanged(IStore store)
        {
            LastStateObserved = ((CounterStore)store).State;
        }
    }

    readonly CounterStore _store = new();
    readonly CounterStateObserver _observer = new();

    [SetUp]
    public void Setup()
    {
        var store = ((IStore)_store);
        store.Listen(_observer);
    }

    [Test]
    public void Test()
    {
        Assert.That(_store.State, Is.EqualTo(new CounterState(0)));

        _store.Increment();

        Assert.Multiple(() =>
        {
            Assert.That(_store.State, Is.EqualTo(new CounterState(1)));

            Assert.That(_observer.LastStateObserved, Is.EqualTo(new CounterState(1)));
        });

        Assert.Pass();
    }
}