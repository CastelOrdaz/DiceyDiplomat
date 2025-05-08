using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiceyDialogue.Cards;

[CreateAssetMenu(fileName = "Card Interaction Data", menuName = "DiceyDialogue/Card Interaction")]
public class CardDisplayData : ScriptableObject
{
    [SerializeField] private CardStateDisplay usableCardDisplay, selectedCardDisplay, usedCardDisplay, unusableCardDisplay;

    public CardStateDisplay UsableCardDisplay { get { return usableCardDisplay; } }
    public CardStateDisplay SelectedCardDisplay { get { return selectedCardDisplay; } }
    public CardStateDisplay UsedCardDisplay {  get { return usedCardDisplay; } }
    public CardStateDisplay UnusableCardDisplay { get { return unusableCardDisplay; } }
}
