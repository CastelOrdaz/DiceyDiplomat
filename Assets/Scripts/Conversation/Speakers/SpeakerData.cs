using DiceyDialogue.Mood;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiceyDialogue.Dialogue;

[CreateAssetMenu(fileName = "New Speaker", menuName = "DiceyDialogue/Speaker")]
public class SpeakerData : ScriptableObject
{
    [SerializeField] protected SpeakerID id;
    [SerializeField] protected Alias[] aliases;
    [SerializeField] protected MoodStatBlock startingStats;
    [SerializeField] protected MoodStatBlock maxStats;
    [SerializeField] protected DialogueData[] dialogue;

    public SpeakerID ID { get { return id; } }
    public Alias[] Aliases { get { return aliases; } }
    public MoodStatBlock StartingStats { get { return startingStats; } }
    public MoodStatBlock MaxStats { get { return maxStats; } }
    public DialogueData[] Dialogue { get {  return dialogue; } }
}
