public static class RegisterEvent
{
    public static void StartListening<TEvent>(this IEventListener<TEvent> caller) where TEvent : struct
    {
        EventBus.AddListener(caller);
    }
    
    public static void StopListening<TEvent>(this IEventListener<TEvent> caller) where TEvent : struct
    {
        EventBus.RemoveListener(caller);
    }
}