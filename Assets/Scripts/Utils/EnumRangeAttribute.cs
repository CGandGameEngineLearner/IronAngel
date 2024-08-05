using UnityEngine;

public class EnumRangeAttribute : PropertyAttribute
{
    public int Min { get; private set; }
    public int Max { get; private set; }

    public EnumRangeAttribute(int min, int max)
    {
        Min = min;
        Max = max;
    }
}