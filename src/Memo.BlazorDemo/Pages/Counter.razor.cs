using Memo;

namespace Memo.BlazorDemo.Pages;


public partial class Counter
{
    record CounterState(int Counter);

    class CounterStore : LocalStore<CounterState>
    {
        protected override CounterState InitialState() => new(0);

        public void Increment() => Mutate(State with { Counter = State.Counter + 1 });
    }
}
