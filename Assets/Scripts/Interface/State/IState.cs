using UniRx;
using UniRx.Triggers;

public interface IState 
{
    ReactiveProperty<bool> WasAttacked { get; set; } 
}
