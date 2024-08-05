#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnumRangeAttribute))]
public class EnumRangeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EnumRangeAttribute range = attribute as EnumRangeAttribute;

        if (property.propertyType == SerializedPropertyType.Enum)
        {
            string[] enumNames = property.enumDisplayNames;
            int[] enumValues = new int[enumNames.Length];

            for (int i = 0; i < enumNames.Length; i++)
            {
                enumValues[i] = i;
            }

            int selectedValue = property.enumValueIndex;
            selectedValue = EditorGUI.IntPopup(position, label.text, selectedValue, GetFilteredNames(enumNames, range), GetFilteredValues(enumValues, range));
            
            property.enumValueIndex = Mathf.Clamp(selectedValue, range.Min, range.Max);
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Use EnumRange with enums.");
        }
    }

    private string[] GetFilteredNames(string[] names, EnumRangeAttribute range)
    {
        int length = range.Max - range.Min + 1;
        string[] filteredNames = new string[length];
        System.Array.Copy(names, range.Min, filteredNames, 0, length);
        return filteredNames;
    }

    private int[] GetFilteredValues(int[] values, EnumRangeAttribute range)
    {
        int length = range.Max - range.Min + 1;
        int[] filteredValues = new int[length];
        System.Array.Copy(values, range.Min, filteredValues, 0, length);
        return filteredValues;
    }
}
#endif