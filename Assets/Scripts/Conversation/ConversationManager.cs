using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DiceyDialogue.Dialogue;
using DiceyDialogue.Mood;
using DiceyDialogue.Cards;
using Unity.VisualScripting;

public class ConversationManager : MonoBehaviour
{
    [SerializeField] private Image dialogueBox;
    [SerializeField] private TMP_Text dialogueText;

    [SerializeField] private Image[] nameTagBoxes;
    [SerializeField] private TMP_Text[] nameTagTexts;

    [SerializeField] private float inactiveSpeakerScale;
    [SerializeField] private Vector3 inactiveSpeakerOffset;

    [SerializeField] private CardHand cardHand;

    private GameObject[] overworldObjects;
    private Speaker[] speakers;

    private DialogueData currentDialogue;
    private int currentSpeaker = -1;
    private string currentTopic;
    private int topicProgress = 0;
    private Card playedCard = null;

    private ConvoState state = ConvoState.Off;

    public bool convoPaused = false;
    private bool win = false;

    public ConvoState State { get { return state; } }
    public bool Win {  get { return win; } }
    public Speaker CurrentSpeaker { get { return speakers[currentSpeaker]; } }

    private void ToggleInterface(bool enable)
    {
        dialogueBox.gameObject.SetActive(enable);

        for (int i = 0; i < speakers.Length; i++)
        {
            speakers[i].gameObject.SetActive(enable);
        }

        for (int i = 0; i < overworldObjects.Length; i++)
        {
            overworldObjects[i].SetActive(!enable);
        }
    }

    public void StartConvo(GameObject[] overworld, Speaker[] newSpeakers, DialogueData firstDialogue)
    {
        gameObject.SetActive(true);

        speakers = newSpeakers;
        for (int i = 0; i < speakers.Length; i++)
        {
            speakers[i].UpdateMood();
        }

        currentDialogue = firstDialogue;
        UpdateConvoData();

        if (currentSpeaker == -1)
        {
            EndConvo();
            return;
        }
        overworldObjects = overworld;
        ToggleInterface(true);

        state = ConvoState.Playing;
    }

    public void AdvanceConvo()
    {
        if (state != ConvoState.Playing)
            return;

        dialogueBox.gameObject.SetActive(true);

        if (currentDialogue == null)
            currentDialogue = speakers[currentSpeaker].DefaultDialogue;

        //apply mood impact to current dialogue listeners
        ImpactedListener[] listeners = currentDialogue.ImpactedListeners;
        for (int i = 0; i < listeners.Length; i++)
        {
            ImpactSpeakerMood(listeners[i].ID, listeners[i].MoodImpact);
        }

        //advance to next step in the convo
        switch (currentDialogue.AdvancementType)
        {
            case DialogueAdvancementType.EndsConvo:
                EndConvo();
                return;

            case DialogueAdvancementType.Continues:
                if (currentDialogue.DialogueBranches[0]  == null)
                {
                    Debug.Log(currentDialogue.name + ": Continued Dialogue not found");
                    currentDialogue = DialogueSearch();
                    break;
                }

                currentDialogue = currentDialogue.DialogueBranches[0];
                break;

            case DialogueAdvancementType.PromptsCards:
                //end convo if card hand is empty
                if (!cardHand.HasCardsOfState(new CardState[] {CardState.Usable, CardState.Selected}))
                {
                    EndConvo();
                    return;
                }

                state = ConvoState.CardPicking;

                cardHand.ToggleActive(true);
                dialogueBox.gameObject.SetActive(false);
                return;

            case DialogueAdvancementType.Branches:
                if (currentDialogue.DialogueBranches.Length == 0)
                {
                    Debug.Log(currentDialogue.name + ": Branched Dialogue not found");
                    currentDialogue = DialogueSearch();
                    break;
                }

                currentDialogue = DialogueSearch(currentDialogue.DialogueBranches);
                break;

            case DialogueAdvancementType.Dynamic:
                currentDialogue = DialogueSearch();
                break;
        }

        UpdateConvoData();
    }

    private void ImpactSpeakerMood(SpeakerID listener, MoodStatValue[] impact)
    {
        int listenerIndex = FindSpeakerIndex(listener);

        for (int i = 0; i < impact.Length; i++)
        {
            speakers[listenerIndex].MoodStats.ChangeStat(impact[i].Stat, impact[i].Value);
        }

        speakers[listenerIndex].UpdateMood();
    }

    private void UpdateConvoData()
    {
        int speakerIndex = FindSpeakerIndex(currentDialogue.Speaker);
        if (speakerIndex == -1)
        {
            Debug.Log(currentDialogue.name + ": Speaker not found (" + currentDialogue.Speaker + ")");
            return;
        }

        if (speakerIndex != currentSpeaker)
        {
            currentSpeaker = speakerIndex;
            UpdateSpeakerDisplay();
        }

        if (currentDialogue.Topic !=  null || currentDialogue.Topic != "")
            currentTopic = currentDialogue.Topic;

        if (currentDialogue.ProgressesTopic)
            topicProgress = currentDialogue.ProgressInTopic + 1;

        dialogueText.text = currentDialogue.Text;
    }

    public void CheckCardPlayed()
    {
        Card card = cardHand.PlayedCard;

        //exit function if there isnt a played card
        if (card == null)
            return;

        //update played card if card played isnt a double down
        if (card.Data.CardID != CardID.DoubleDown)
            playedCard = card;

        cardHand.DoubleDownPlayable = true;

        state = ConvoState.Playing;
        cardHand.ToggleActive(false);
        ToggleInterface(true);

        ImpactSpeakerMood(SpeakerID.Dicey, playedCard.Data.DiceyMoodImpact);
        UpdateSpeakerDisplay();

        if (currentDialogue.CardOptions == null)
            currentDialogue = DialogueSearch();
        else
            currentDialogue = DialogueSearch(currentDialogue.CardOptions);
        UpdateConvoData();
    }

    public DialogueData DialogueSearch(DialogueData[] options)
    {
        if (options == null || options.Length == 0)
        {
            Debug.Log(currentDialogue.name + ": Dialogue options not found");
            return speakers[currentSpeaker].DefaultDialogue;
        }

        int mostChecksPassed = 0;
        int bestOptionIndex = 0;

        for (int i = 0; i < options.Length; i++)
        {
            int checksPassed = 0;

            //dialogue option's value increases with more checks it passes
            if (options[i].Topic == currentTopic)
                checksPassed++;
            if (options[i].ProgressInTopic == topicProgress)
                checksPassed++;
            
            for (int check = 0; check < options[i].MoodCheck.Length; check++)
            {
                if (options[i].MoodCheck[check] == Mood.None)
                    continue;
                if (options[i].MoodCheck[check] == speakers[FindSpeakerIndex(options[i].Speaker)].CurrentMood)
                {
                    checksPassed++;
                    break;
                }
            }

            for (int check = 0; check < options[i].PlayedCardCheck.Length; check++)
            {
                if (options[i].PlayedCardCheck[check] == CardID.None)
                    continue;
                if (options[i].PlayedCardCheck[check] == playedCard.Data.CardID)
                {
                    checksPassed += 4;
                    break;
                }
            }

            //current option becomes the best option if it passes more checks than the previous options
            if (checksPassed > mostChecksPassed)
            {
                mostChecksPassed = checksPassed;
                bestOptionIndex = i;
            }
        }
        return options[bestOptionIndex];
    }

    public DialogueData DialogueSearch(CardOptionsData[] cardOptionsData)
    {
        for (int i = 0; i < cardOptionsData.Length; i++)
        {
            CardID[] checks = cardOptionsData[i].CardChecks;
            for (int card = 0; card < checks.Length; card++)
            {
                if (cardOptionsData[i].CardChecks[card] == playedCard.Data.CardID)
                    return DialogueSearch(cardOptionsData[i].Dialogue);
            }
        }

        Debug.Log("Could not find valid card dialogue");
        return DialogueSearch();
    }

    public DialogueData DialogueSearch()
    {
        return DialogueSearch(speakers[currentSpeaker].Data.Dialogue);
    }

    public int FindSpeakerIndex(SpeakerID speakerID)
    {
        if (speakerID == SpeakerID.None)
            return currentSpeaker;

        for(int i = 0;i < speakers.Length;i++)
        {
            if (speakers[i].Data.ID == speakerID)
                return i;
        }

        return -1;
    }

    public void UpdateSpeakerDisplay()
    {
        //tell each speaker if they're actively speaking or not
        for (int i = 0; i < speakers.Length; i++)
        {
            speakers[i].ToggleActiveSpeaker(i == currentSpeaker, inactiveSpeakerScale, inactiveSpeakerOffset);
        }

        Color textColor = speakers[currentSpeaker].TextColor;
        string displayName = speakers[currentSpeaker].DisplayName;

        //activate appropriate name tag back
        for (int i = 0; i < nameTagBoxes.Length; i++)
        {
            if (i != currentSpeaker)
            {
                nameTagBoxes[i].gameObject.SetActive(false);
                continue;
            }

            nameTagBoxes[i].gameObject.SetActive(true);

            nameTagTexts[i].text = displayName;
            nameTagTexts[i].color = textColor;
            nameTagBoxes[i].color = textColor;
        }

        dialogueText.color = textColor;
        dialogueBox.color = textColor;
    }

    public void EndConvo()
    {
        state = ConvoState.Ended;
        win = currentDialogue.Topic == "Over the Wall";

        currentSpeaker = -1;
        currentDialogue = null;

        cardHand.ResetHand();
        cardHand.ToggleActive(false);

        ToggleInterface(false);
        gameObject.SetActive(false);
    }

    public void Start()
    {
        gameObject.SetActive(false);
    }

    public void Update()
    {
        if (convoPaused)
            return;

        switch (state)
        {
            default:
                break;

            case ConvoState.Playing:
                if (Input.GetButtonDown("Interact"))
                    AdvanceConvo();
                break;

            case ConvoState.CardPicking:
                CheckCardPlayed();
                break;
        }
    }
}
