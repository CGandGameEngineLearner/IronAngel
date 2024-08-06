using System.Collections.Generic;
using System;

//事件广播中心
public static class EventCenter
{
    static Dictionary<EventType, Delegate> m_EventListeners = new Dictionary<EventType, Delegate>();
//public--------------------------------------------------------------------
    public static void AddListener(EventType eventType, Callback callback)
    {
        Delegate previous;
        if (CheckPreviousListeners(eventType, callback, out previous))
            m_EventListeners[eventType] = (Callback)previous + callback;
    }

    public static void AddListener<T1>(EventType eventType, Callback<T1> callback)
    {
        Delegate previous;
        if (CheckPreviousListeners(eventType, callback, out previous))
            m_EventListeners[eventType] = (Callback<T1>)previous + callback;
    }

    public static void AddListener<T1,T2>(EventType eventType, Callback<T1,T2> callback)
    {
        Delegate previous;
        if (CheckPreviousListeners(eventType, callback, out previous))
            m_EventListeners[eventType] = (Callback<T1,T2>)previous + callback;
    }

    public static void AddListener<T1,T2,T3>(EventType eventType, Callback<T1,T2,T3> callback)
    {
        Delegate previous;
        if (CheckPreviousListeners(eventType, callback, out previous))
            m_EventListeners[eventType] = (Callback<T1,T2,T3>)previous + callback;
    }

    public static void AddListener<T1,T2,T3,T4>(EventType eventType, Callback<T1,T2,T3,T4> callback)
    {
        Delegate previous;
        if (CheckPreviousListeners(eventType, callback, out previous))
            m_EventListeners[eventType] = (Callback<T1,T2,T3,T4>)previous + callback;
    }

    public static void RemoveListener(EventType eventType, Callback callback)
    {
        Delegate previous;
        if (CheckPreviousListeners(eventType, callback, out previous) && previous != null)
            m_EventListeners[eventType] = (Callback)m_EventListeners[eventType] - callback;
        AfterListenerRemoved(eventType);
    }
    public static void RemoveListener<T1>(EventType eventType, Callback<T1> callback)
    {
        Delegate previous;
        if (CheckPreviousListeners(eventType, callback, out previous) && previous != null)
            m_EventListeners[eventType] = (Callback<T1>)m_EventListeners[eventType] - callback;
        AfterListenerRemoved(eventType);
    }

    public static void RemoveListener<T1,T2>(EventType eventType, Callback<T1,T2> callback)
    {
        Delegate previous;
        if (CheckPreviousListeners(eventType, callback, out previous) && previous != null)
            m_EventListeners[eventType] = (Callback<T1,T2>)m_EventListeners[eventType] - callback;
        AfterListenerRemoved(eventType);
    }

    public static void RemoveListener<T1,T2,T3>(EventType eventType, Callback<T1,T2,T3> callback)
    {
        Delegate previous;
        if (CheckPreviousListeners(eventType, callback, out previous) && previous != null)
            m_EventListeners[eventType] = (Callback<T1,T2,T3>)m_EventListeners[eventType] - callback;
        AfterListenerRemoved(eventType);
    }

    public static void RemoveListener<T1,T2,T3,T4>(EventType eventType, Callback<T1,T2,T3,T4> callback)
    {
        Delegate previous;
        if (CheckPreviousListeners(eventType, callback, out previous) && previous != null)
            m_EventListeners[eventType] = (Callback<T1,T2,T3,T4>)m_EventListeners[eventType] - callback;
        AfterListenerRemoved(eventType);
    }

    public static void Broadcast(EventType eventType)
    {
        Delegate listener;
        if(m_EventListeners.TryGetValue(eventType, out listener))
        {
            Callback callback = listener as Callback;
            callback?.Invoke();
        }
    }
 
    public static void Broadcast<T1>(EventType eventType, T1 arg1)
    {
        Delegate listener;
        if(m_EventListeners.TryGetValue(eventType, out listener))
        {
            Callback<T1> callback = listener as Callback<T1>;
            callback?.Invoke(arg1);
        }
    }

    public static void Broadcast<T1,T2>(EventType eventType, T1 arg1, T2 arg2)
    {
        Delegate listener;
        if(m_EventListeners.TryGetValue(eventType, out listener))
        {
            Callback<T1,T2> callback = listener as Callback<T1,T2>;
            callback?.Invoke(arg1,arg2);
        }
    }

    public static void Broadcast<T1,T2,T3>(EventType eventType, T1 arg1, T2 arg2, T3 arg3)
    {
        Delegate listener;
        if(m_EventListeners.TryGetValue(eventType, out listener))
        {
            Callback<T1,T2,T3> callback = listener as Callback<T1,T2,T3>;
            callback?.Invoke(arg1,arg2,arg3);
        }
    }

    public static void Broadcast<T1,T2,T3,T4>(EventType eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        Delegate listener;
        if(m_EventListeners.TryGetValue(eventType, out listener))
        {
            Callback<T1,T2,T3,T4> callback = listener as Callback<T1,T2,T3,T4>;
            callback?.Invoke(arg1,arg2,arg3,arg4);
        }
    }


//private--------------------------------------------------------------------
    private static bool CheckPreviousListeners(EventType eventType, Delegate callback, out Delegate previous)
    {
        if (!m_EventListeners.TryGetValue(eventType, out previous) || previous.GetType() == callback.GetType())
            return true;
        throw new Exception($"EventCenter listener type mismatched: {eventType}");
    }

    private static void AfterListenerRemoved(EventType eventType)
    {
        if (!m_EventListeners.ContainsKey(eventType))
        {
            return;
        }
        if (m_EventListeners[eventType] == null)
            m_EventListeners.Remove(eventType);
    }
}
