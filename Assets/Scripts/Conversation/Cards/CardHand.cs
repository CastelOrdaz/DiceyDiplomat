using DiceyDialogue.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHand : MonoBehaviour
{
    private List<Card> cards;
    [SerializeField] private GameObject cardPrefab;

    [SerializeField] private float angleBetweenCards;
    [SerializeField] private float maxHandAngle;
    [SerializeField] private float spaceBetweenCards;

    private int selectedCard = -1;
    private Card playedCard = null;
    private bool doubleDownPlayable = false;

    public bool handPaused = false;

    public bool DoubleDownPlayable { set { doubleDownPlayable = value; } }

    public Card PlayedCard { get { return playedCard; } }

    public void ToggleActive(bool active)
    {
        gameObject.SetActive(active);
        playedCard = null;
        selectedCard = -1;
    }

    public void AddCard(CardData newCardData)
    {
        Card card = Instantiate(cardPrefab, transform).GetComponent<Card>();
        card.SetData(newCardData);

        cards.Add(card);
        CardSpread();
    }

    public void CardSpread()
    {
        bool tooManyCards = cards.Count * angleBetweenCards > maxHandAngle;

        for (int i = 0; i < cards.Count; i++)
        {
            float angle = i - ((cards.Count - 1) * .5f);
            if (tooManyCards)
                angle *= maxHandAngle/cards.Count;
            else
                angle *= angleBetweenCards;

            Vector3 cardRotation = cards[i].transform.eulerAngles;
            Vector3 cardPostion = cards[i].transform.localPosition;
            cards[i].transform.eulerAngles = new Vector3(cardRotation.x, cardRotation.y, -angle);
            cards[i].transform.localPosition = new Vector3(cardPostion.x, cardPostion.y, i * spaceBetweenCards);
        }
    }

    public void SelectCard(Card hoveredCard)
    {
        //reset selected card if hovered card doesnt exist
        if (hoveredCard == null)
        {
            if (selectedCard != -1)
            {
                cards[selectedCard].SetState(CardState.Usable);
                selectedCard = -1;
            }
            return;
        }

        //end function if hovered card is not usable
        if (hoveredCard.State == CardState.Unusable)
            return;
        if (hoveredCard.State == CardState.Used)
            return;

        //select hovered card and unselect any previously selected cards
        for (int i = 0;i < cards.Count;i++)
        {
            if (cards[i] == hoveredCard)
            {
                selectedCard = i;
                cards[i].SetState(CardState.Selected);
                continue;
            }

            if (cards[i].State == CardState.Selected)
            {
                cards[i].SetState(CardState.Usable);
            }
        }
    }

    private void UseCard()
    {
        if (selectedCard == -1)
            return;

        if (Input.GetButtonDown("Use"))
        {
            Card usedCard = cards[selectedCard];

            usedCard.SetState(CardState.Used);

            switch (usedCard.Data.CardID)
            {
                //set used card ID
                default:
                    playedCard = usedCard;
                    break;

                //set all non-breathe cards to usable
                case CardID.Breathe:
                    for (int i = 0; i < cards.Count; i++)
                    {
                        if (cards[i].Data.CardID != CardID.Breathe)
                            cards[i].SetState(CardState.Usable);
                    }
                    break;
            }

            selectedCard = -1;
        }
    }

    private void UpdateUsableCards()
    {
        //set unusable breathe cards to usable if there are used cards
        if (HasCardsOfState(CardState.Used))
            changeCardStates(CardID.Breathe, CardState.Unusable, CardState.Usable);
        //set usable breathe cards to unusable if there are no used cards
        else
            changeCardStates(CardID.Breathe, CardState.Usable, CardState.Unusable);

        //set unusable double down cards to usable if they can be played
        if (doubleDownPlayable)
            changeCardStates(CardID.DoubleDown, CardState.Unusable, CardState.Usable);
        //set usable double down cards to unusable if they cant be played
        else
            changeCardStates(CardID.DoubleDown, CardState.Usable, CardState.Unusable);
    }

    private void changeCardStates(CardID cardType, CardState stateToChange, CardState newState)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            //skip card if its not the right type or state
            if (cards[i].Data.CardID != cardType)
                continue;
            if (cards[i].State != stateToChange)
                continue;

            cards[i].SetState(newState);
        }
    }

    public bool HasCardsOfState(CardState searchedState)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            //skip breathe cards if looking for used cards
            if (cards[i].Data.CardID == CardID.Breathe && searchedState == CardState.Used)
                continue;

            if (cards[i].State == searchedState)
                return true;
        }
        return false;
    }

    public bool HasCardsOfState(CardState[] searchStates)
    {
        for (int i = 0; i < searchStates.Length; i++)
        {
            if (HasCardsOfState(searchStates[i]))
                return true;
        }

        return false;
    }

    public void ResetHand()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].SetState(CardState.Usable);
            doubleDownPlayable = false;
        }
    }

    public void Awake()
    {
        cards = new List<Card>();
        ToggleActive(false);
    }

    private void Update()
    {
        if (handPaused)
            return;

        UseCard();
        UpdateUsableCards();
    }
}
