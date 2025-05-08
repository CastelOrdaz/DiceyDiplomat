using DiceyDialogue.Cards;
using DiceyDialogue.Dialogue;
using DiceyDialogue.Mood;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Options", menuName = "DiceyDialogue/Dialogue/Card Options")]
public class CardOptionsData : ScriptableObject
{
    [SerializeField] CardID[] cardChecks;
    [SerializeField] DialogueData[] dialogue;

    public CardID[] CardChecks {  get { return cardChecks; } }
    public DialogueData[] Dialogue { get {  return dialogue; } }
}
