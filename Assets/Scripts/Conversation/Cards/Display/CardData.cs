using DiceyDialogue.Dialogue;
using DiceyDialogue.Mood;
using DiceyDialogue.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "DiceyDialogue/Card")]
public class CardData : ScriptableObject
{
    [SerializeField] private CardID id;
    [SerializeField] private string displayName;
    [SerializeField] private string[] blurbs;
    [SerializeField] private Sprite image;
    [SerializeField] private MoodStatValue[] diceyMoodImpact;

    public CardID CardID { get { return id; } }
    public string DisplayName {  get { return displayName; } }
    public string[] Blurbs { get {  return blurbs; } }
    public MoodStatValue[] DiceyMoodImpact { get {  return diceyMoodImpact; } }
    public Sprite Image { get { return image; } }
}