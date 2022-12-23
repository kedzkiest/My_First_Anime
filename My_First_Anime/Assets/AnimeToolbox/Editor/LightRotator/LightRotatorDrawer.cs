using UnityEditor;
using UnityEngine;

namespace Unity.AnimeToolbox.Editor {

[CustomPropertyDrawer(typeof(LightRotatorBehaviour))]
public class LightRotatorDrawer : PropertyDrawer
{
    public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
    {
        int fieldCount = 1;
        return fieldCount * EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty eulerAnglesProp = property.FindPropertyRelative("eulerAngles");

        Rect singleFieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(singleFieldRect, eulerAnglesProp);
    }
}

} //end namespace
