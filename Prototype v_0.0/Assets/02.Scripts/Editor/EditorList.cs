using UnityEditor;
using UnityEngine;
/*
public static class EditorList
{
    private static GUIContent
        selectButtonContent = new GUIContent("Select", "Select");

    public static void Show(SerializedProperty list, EditorListOptions options = EditorListOptions.Default)
    {
        bool
            showListLabel = (options & EditorListOptions.ListLabel) != 0,
            showListSize = (options & EditorListOptions.ListSize) != 0;

        if (showListLabel)
        {
            EditorGUILayout.PropertyField(list);
            EditorGUI.indentLevel += 1;
        }
        if (!showListLabel || list.isExpanded)
        {
            if (showListSize)
            {
                EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
            }
            ShowElements(list, options);
        }
        if (showListLabel)
        {
            EditorGUI.indentLevel -= 1;
        }
    }
    private static void ShowElements(SerializedProperty list, EditorListOptions options)
    {
        bool
            showElementLabels = (options & EditorListOptions.ElementLabels) != 0,
            showButtons = (options & EditorListOptions.Buttons) != 0;

        for (int i = 0; i < list.arraySize; i++)
        {
            if (showButtons)
            {
                EditorGUILayout.BeginHorizontal();
            }
            if (showElementLabels)
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
            }
            else
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none);
            }
            if (showButtons)
            {
                ShowButtons(list, i);
                EditorGUILayout.EndHorizontal();
            }
        }
    }

    private static void ShowButtons(SerializedProperty list, int index)
    {
        if (GUILayout.Button(selectButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth))
        {
            MapToolEditor.selected = index;
        }
    }

    private static GUILayoutOption miniButtonWidth = GUILayout.Width(50f);

    
}
*/
