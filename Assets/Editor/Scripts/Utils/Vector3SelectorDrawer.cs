
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Vector3))]
public class Vector3SelectorDrawer : PropertyDrawer
{
    private bool isSelecting = false;
    private SerializedProperty currentProperty;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Draw the default Vector3 field
        Rect vectorFieldRect = new Rect(position.x, position.y, position.width - 200, position.height);
        EditorGUI.PropertyField(vectorFieldRect, property, label);

        // Draw the "Select from Scene" button
        Rect selectButtonRect = new Rect(position.x + position.width - 190, position.y, 90, position.height);
        if (GUI.Button(selectButtonRect, "Select Pos"))
        {
            isSelecting = true;
            currentProperty = property;
            SceneView.duringSceneGui += OnSceneGUI;
        }

        // Draw the "Focus" button
        Rect focusButtonRect = new Rect(position.x + position.width - 90, position.y, 90, position.height);
        if (GUI.Button(focusButtonRect, "Focus"))
        {
            FocusSceneViewOnPoint(property.vector3Value);
        }

        EditorGUI.EndProperty();
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        if (isSelecting && e.type == UnityEngine.EventType.MouseDown && e.button == 0)
        {
            Vector3 mousePosition = e.mousePosition;
            mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y; // Invert y axis
            Vector3 worldPosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePosition);
            worldPosition.z = 0; // Ensure z is always 0

            currentProperty.vector3Value = worldPosition;
            currentProperty.serializedObject.ApplyModifiedProperties();

            isSelecting = false;
            SceneView.duringSceneGui -= OnSceneGUI;
            e.Use();
        }
        else if (e.type == UnityEngine.EventType.MouseDown && e.button != 0)
        {
            isSelecting = false;
            SceneView.duringSceneGui -= OnSceneGUI;
        }
    }

    private void FocusSceneViewOnPoint(Vector3 point)
    {
        SceneView.lastActiveSceneView.LookAt(point, SceneView.lastActiveSceneView.rotation, 10f);
        SceneView.lastActiveSceneView.Repaint();
    }
}