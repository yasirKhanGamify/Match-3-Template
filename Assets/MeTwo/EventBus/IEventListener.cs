public interface IEventListener<TEvent>: IEventListenerBase
{
    void OnEvent(TEvent e);
}