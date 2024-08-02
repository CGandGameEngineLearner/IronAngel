using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
public class ConditionalHidePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

        if (enabled)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

        return enabled ? EditorGUI.GetPropertyHeight(property, label) : 0f;
    }

    private bool GetConditionalHideAttributeResult(ConditionalHideAttribute condHAtt, SerializedProperty property)
    {
        SerializedProperty sourceProperty = property.serializedObject.FindProperty(condHAtt.EnumFieldName);

        if (sourceProperty != null && sourceProperty.propertyType == SerializedPropertyType.Enum)
        {
            return sourceProperty.enumValueIndex == condHAtt.EnumValue;
        }
        else
        {
            Debug.LogWarning($"Couldn't find or invalid type for property: {condHAtt.EnumFieldName}");
            return false;
        }
    }
}