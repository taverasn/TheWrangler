using EasyBuildSystem.Features.Runtime.Buildings.Part.Conditions;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuildingItemCondition))]
public class BuildingItemConditionEditor : UnityEditor.Editor
{
    #region Fields

    BuildingItemCondition Target
    {
        get
        {
            return ((BuildingItemCondition)target);
        }
    }

    #endregion

    #region Unity Methods

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Recipe"), new GUIContent("Recipe",
            "Needed items for crafting building."));

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }

    #endregion
}
