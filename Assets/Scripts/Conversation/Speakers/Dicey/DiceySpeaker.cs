using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiceyDialogue.Mood;

public class DiceySpeaker : Speaker
{
    [SerializeField] CardOptionsData cardOptionsData;
    [SerializeField] float RollTime;
    private float RollCooldown;

    private Mood previousMood = Mood.None;

    public override void UpdateMood()
    {
        previousMood = currentMood;

        base.UpdateMood();
        UpdateAlias((int)currentMood);

        if (previousMood != currentMood || RollCooldown != 0)
        {
            RollCooldown = RollTime;
            previousMood = currentMood;
            animating = true;

            spriteRenderer.sprite = data.Aliases[0].SpeakerSprites[0].Image;
        }
    }

    public void Update()
    {
        if (animating)
        {
            RollCooldown -= Time.deltaTime;
        }

        if (RollCooldown < 0)
        {
            RollCooldown = 0;
            animating = false;

            UpdateAlias((int) currentMood);
        }

        DebugDiceyMood();
    }

    public void DebugDiceyMood()
    {
        //dirty, dirty code
        bool change = false;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            moodStats.ChangeStat(MoodStat.Confidence, 4);
            moodStats.ChangeStat(MoodStat.Agreeability, 4);
            moodStats.ChangeStat(MoodStat.Abrasiveness, -4);
            moodStats.ChangeStat(MoodStat.Deceitfulness, -4);
            change = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            moodStats.ChangeStat(MoodStat.Confidence, 4);
            moodStats.ChangeStat(MoodStat.Agreeability, -4);
            moodStats.ChangeStat(MoodStat.Abrasiveness, 4);
            moodStats.ChangeStat(MoodStat.Deceitfulness, -4);
            change = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            moodStats.ChangeStat(MoodStat.Confidence, 4);
            moodStats.ChangeStat(MoodStat.Agreeability, -4);
            moodStats.ChangeStat(MoodStat.Abrasiveness, -4);
            moodStats.ChangeStat(MoodStat.Deceitfulness, 4);
            change = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            moodStats.ChangeStat(MoodStat.Confidence, -4);
            moodStats.ChangeStat(MoodStat.Agreeability, -4);
            moodStats.ChangeStat(MoodStat.Abrasiveness, -4);
            moodStats.ChangeStat(MoodStat.Deceitfulness, 4);
            change = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            moodStats.ChangeStat(MoodStat.Confidence, -4);
            moodStats.ChangeStat(MoodStat.Agreeability, -4);
            moodStats.ChangeStat(MoodStat.Abrasiveness, 4);
            moodStats.ChangeStat(MoodStat.Deceitfulness, -4);
            change = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            moodStats.ChangeStat(MoodStat.Confidence, -4);
            moodStats.ChangeStat(MoodStat.Agreeability, 4);
            moodStats.ChangeStat(MoodStat.Abrasiveness, -4);
            moodStats.ChangeStat(MoodStat.Deceitfulness, -4);
            change = true;
        }

        if (change)
        {
            UpdateMood();
        }
    }
}
