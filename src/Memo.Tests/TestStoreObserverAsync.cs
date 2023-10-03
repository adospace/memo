namespace Memo.Tests;

public class TestStoreObserverAsync
{
    record CounterState(int Counter);

    class CounterStore : LocalStore<CounterState>
    {
        protected override CounterState InitialState() => new(0);

        public async Task Increment()
        {
            await Task.Delay(1000);

            Mutate(State with { Counter = State.Counter + 1 });
        }
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
    public async Task Test()
    {
        Assert.That(_store.State, Is.EqualTo(new CounterState(0)));

        await _store.Increment();

        Assert.Multiple(() =>
        {
            Assert.That(_store.State, Is.EqualTo(new CounterState(1)));

            Assert.That(_observer.LastStateObserved, Is.EqualTo(new CounterState(1)));
        });

        Assert.Pass();
    }
}