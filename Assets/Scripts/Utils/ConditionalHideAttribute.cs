using UnityEngine;

public class ConditionalHideAttribute : PropertyAttribute
{
    public string EnumFieldName { get; private set; }
    public int EnumValue { get; private set; }

    public ConditionalHideAttribute(string enumFieldName, int enumValue)
    {
        this.EnumFieldName = enumFieldName;
        this.EnumValue = enumValue;
    }
}