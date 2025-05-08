using DiceyDialogue.Dialogue;
using DiceyDialogue.Mood;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour
{
    [SerializeField] protected SpeakerData data;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    protected bool isActiveSpeaker;
    protected Vector3 originalScale, originalPos;
    protected float currentScale;

    protected DynamicMoodStatBlock moodStats;
    protected Mood currentMood;
    protected int currentAlias;

    protected bool animating;

    public SpeakerData Data { get { return data; } }
    public DialogueData DefaultDialogue { get { return data.Dialogue[0]; } }

    public MoodStatBlock MoodStats { get { return moodStats; } }

    public Mood CurrentMood { get { return currentMood; } }

    public string DisplayName { get { return data.Aliases[currentAlias].DisplayName; } }
    public Color TextColor { get {  return data.Aliases[currentAlias].TextColor; } }
    public bool Animating { get { return animating; } }

    public void ToggleActiveSpeaker(bool active, float inactiveScale, Vector3 inactiveOffset)
    {
        if (originalScale == Vector3.zero)
        {
            originalScale = transform.localScale;
        }
        if (originalPos == Vector3.zero)
        {
            originalPos = transform.position;
        }

        isActiveSpeaker = active;

        if (isActiveSpeaker)
        {
            transform.localScale = originalScale;
            transform.position = originalPos;
        }
        else
        {
            transform.localScale = originalScale * inactiveScale;
            transform.position = originalPos + inactiveOffset;
        }
    }

    public virtual void UpdateMood()
    {
        if (moodStats == null)
        {
            moodStats = new DynamicMoodStatBlock(data.StartingStats, data.MaxStats);
        }

        //check if confidence is atleast half
        bool highConfidence = moodStats.GetStat(MoodStat.Confidence) > 0;

        //check for highest mood stat (confidence and patience not included)
        float highestValue = 0;
        MoodStat highestStat = MoodStat.Agreeability;
        for (MoodStat stat = MoodStat.Agreeability; stat < MoodStat.Deceitfulness + 1; stat++)
        {
            float statValue = moodStats.GetStat(stat);

            if (statValue > highestValue)
            {
                highestValue = statValue;
                highestStat = stat;
            }
        }

        switch (highConfidence, highestStat)
        {
            case (true, MoodStat.Agreeability):
                currentMood = Mood.Cheery;
                break;

            case (true, MoodStat.Abrasiveness):
                currentMood = Mood.Stoic;
                break;

            case (true, MoodStat.Deceitfulness):
                currentMood = Mood.Sneaky;
                break;

            case (false, MoodStat.Agreeability):
                currentMood = Mood.Pitiful;
                break;

            case (false, MoodStat.Abrasiveness):
                currentMood = Mood.Angry;
                break;

            case (false, MoodStat.Deceitfulness):
                currentMood = Mood.Nervous;
                break;
        }

        UpdateSpeakerSprite();
    }

    public void UpdateAlias(int aliasIndex)
    {
        currentAlias = aliasIndex; ;
        UpdateSpeakerSprite();
    }

    protected void UpdateSpeakerSprite()
    {
        SpeakerSprite[] sprites = data.Aliases[currentAlias].SpeakerSprites;
        spriteRenderer.sprite = sprites[0].Image;

        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].MoodVariant == currentMood)
            {
                spriteRenderer.sprite = sprites[i].Image;
            }
        }
    }

    public void Awake()
    {
        UpdateMood();
    }
}
