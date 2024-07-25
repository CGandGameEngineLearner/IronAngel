//事件列表
public enum EventType
{
    None,
}

public delegate void Callback();
public delegate void Callback<T1>(T1 arg1);

public delegate void Callback<T1,T2>(T1 arg1,T2 arg2);