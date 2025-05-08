using DiceyDialogue.Cards;
using DiceyDialogue.Dialogue;
using DiceyDialogue.Mood;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "DiceyDialogue/Dialogue/Single")]
public class DialogueData : ScriptableObject
{
    [SerializeField] private SpeakerID speaker;
    [SerializeField] private ImpactedListener[] impactedListeners;
    [SerializeField] private string topic, text;
    [SerializeField] private int progressInTopic;
    [SerializeField] private bool progressesTopic;
    [SerializeField] private Mood[] moodCheck;
    [SerializeField] private CardID[] playedCardCheck;

    [SerializeField] private DialogueAdvancementType advancementType;
    [SerializeField] private DialogueData[] dialogueBranches;
    [SerializeField] private CardOptionsData[] cardOptions;

    public SpeakerID Speaker { get { return speaker; } }
    public ImpactedListener[] ImpactedListeners { get { return impactedListeners; } }
    public string Topic { get { return topic; } }
    public string Text { get { return text; } }
    public Mood[] MoodCheck { get { return moodCheck; } }
    public int ProgressInTopic { get {  return progressInTopic; } }

    public bool ProgressesTopic { get { return progressesTopic; } }
    public DialogueAdvancementType AdvancementType { get { return advancementType; } }
    public DialogueData[] DialogueBranches {  get { return dialogueBranches; } }
    public CardID[] PlayedCardCheck { get { return playedCardCheck;} }
    public CardOptionsData[] CardOptions { get { return cardOptions; } }
}
