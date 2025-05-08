using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEditor;
using DiceyDialogue.Mood;
using Unity.VisualScripting;

[CustomPropertyDrawer(typeof(MoodStatBlock))]
public class MoodStatBlockEditor : PropertyDrawer
{
    bool showFoldout;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Rect foldoutRect = new Rect (position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        showFoldout = EditorGUI.Foldout(foldoutRect, showFoldout, label, true);

        if (showFoldout)
        {
            EditorGUI.indentLevel++;

            SerializedProperty stats = property.FindPropertyRelative("stats");
            stats.arraySize = 5;

            string statLabel = "";
            for (int i = 0; i < stats.arraySize; ++i)
            {
                switch(i)
                {
                    case 0:
                        statLabel = "Patience";
                        break;
                    case 1:
                        statLabel = "Confidence";
                        break;
                    case 2:
                        statLabel = "Agreeability";
                        break;
                    case 3:
                        statLabel = "Abrasiveness";
                        break;
                    case 4:
                        statLabel = "Deceitfulness";
                        break;
                }
                stats.GetArrayElementAtIndex(i).floatValue = EditorGUILayout.FloatField(statLabel, stats.GetArrayElementAtIndex(i).floatValue);
            }

            EditorGUI.indentLevel--;
        }
    }
}
