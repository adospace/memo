using Memo;

namespace Memo.BlazorDemo.Pages;


public partial class Counter
{
    record CounterState(int Counter);

    class CounterStore : LocalStore<CounterState>
    {
        public CounterStore() : base(() => new(0))
        {
        }

        public void Increment() => Mutate(s => s with { Counter = State.Counter + 1 });
    }
}
