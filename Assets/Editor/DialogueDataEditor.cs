using DiceyDialogue.Dialogue;
using DiceyDialogue.Mood;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueData))]
public class DialogueDataEditor : Editor
{
    private SerializedProperty 
        speaker, impactedListeners, topic, text, progressInTopic, progressesTopic, moodCheck, advancementType, dialogueBranches, playedCardCheck, cardOptions;

    private DialogueAdvancementType type;

    public void OnEnable()
    {
        speaker = serializedObject.FindProperty("speaker");
        topic = serializedObject.FindProperty("topic");
        text = serializedObject.FindProperty("text");
        progressInTopic = serializedObject.FindProperty("progressInTopic");
        progressesTopic = serializedObject.FindProperty("progressesTopic");
        impactedListeners = serializedObject.FindProperty("impactedListeners");
        moodCheck = serializedObject.FindProperty("moodCheck");
        playedCardCheck = serializedObject.FindProperty("playedCardCheck");
        advancementType = serializedObject.FindProperty("advancementType");
        dialogueBranches = serializedObject.FindProperty("dialogueBranches");
        cardOptions = serializedObject.FindProperty("cardOptions");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(speaker);
        EditorGUILayout.PropertyField(impactedListeners);
        EditorGUILayout.PropertyField(text);

        EditorGUILayout.PropertyField(topic);
        EditorGUILayout.PropertyField(progressInTopic);
        EditorGUILayout.PropertyField(progressesTopic);

        EditorGUILayout.PropertyField(moodCheck);
        EditorGUILayout.PropertyField(playedCardCheck);

        EditorGUILayout.PropertyField(advancementType);
        type = (DialogueAdvancementType)advancementType.enumValueFlag;

        EditorGUI.indentLevel++;
        switch (type)
        {
            case DialogueAdvancementType.Continues:
                if (dialogueBranches.arraySize == 0)
                    dialogueBranches.arraySize = 1;

                EditorGUILayout.PropertyField(dialogueBranches.GetArrayElementAtIndex(0), new GUIContent("Next Dialogue"));
                break;

            case DialogueAdvancementType.Branches:
                EditorGUILayout.PropertyField(dialogueBranches);
                break;

            case DialogueAdvancementType.PromptsCards:
                EditorGUILayout.PropertyField(cardOptions);
                break;
        }
        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }
}
