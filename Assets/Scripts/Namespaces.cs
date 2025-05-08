using DiceyDialogue.Dialogue;
using DiceyDialogue.Mood;
using System;
using UnityEngine;

namespace DiceyDialogue.Mood
{
    public enum MoodStat
    {
        Patience,
        Confidence,
        Agreeability,
        Abrasiveness,
        Deceitfulness
    }

    public enum Mood
    {
        None,
        Cheery,
        Stoic,
        Sneaky,
        Nervous,
        Angry,
        Pitiful
    }

    [Serializable]
    public struct MoodStatValue
    {
        [SerializeField] private MoodStat stat;
        [SerializeField] private float value;

        public MoodStat Stat { get { return stat; } }
        public float Value { get { return value; } }
    }

    [Serializable]
    public class MoodStatBlock
    {
        [SerializeField] protected float[] stats;

        public float GetStat(MoodStat stat)
        {
            return stats[(int)stat];
        }

        public void ChangeStat(MoodStat stat, float value)
        {
            SetStat(stat, GetStat(stat) + value);
        }

        public void ScaleStat(MoodStat stat, float scale)
        {
            SetStat(stat, GetStat(stat) * scale);
        }

        public virtual void SetStat(MoodStat stat, float value)
        {
            stats[(int)stat] = value;
        }

        public MoodStatBlock()
        {
            stats = new float[5];
        }
    }

    public class DynamicMoodStatBlock : MoodStatBlock
    {
        private float[] maxStats;

        public DynamicMoodStatBlock(MoodStatBlock startingValues, MoodStatBlock maxValues)
        {
            stats = new float[5];
            maxStats = new float[5];

            for (MoodStat stat = 0; stat < MoodStat.Deceitfulness + 1; stat++)
            {
                stats[(int) stat] = startingValues.GetStat(stat);
                maxStats[(int)stat] = maxValues.GetStat(stat);
            }
        }

        public float GetMaxStat(MoodStat stat)
        {
            return maxStats[(int)stat];
        }

        public override void SetStat(MoodStat stat, float value)
        {
            base.SetStat(stat, value);
            ClampStat(stat);
        }

        private void ClampStat(MoodStat stat)
        {
            float value = GetStat(stat);
            float max = GetMaxStat(stat);

            if (value > max)
            {
                base.SetStat(stat, max);
                return;
            }
            if (value < -max)
            {
                base.SetStat(stat, -max);
                return;
            }
        }
    }
}

namespace DiceyDialogue.Dialogue
{
    public enum SpeakerID
    {
        None,
        Dicey,
        RatBurglar
    }

    public enum DialogueAdvancementType
    {
        Dynamic,
        Continues,
        Branches,
        EndsConvo,
        PromptsCards
    }
    public enum ConvoState
    {
        Off,
        Playing,
        CardPicking,
        Ended
    }

    [Serializable]
    public struct ImpactedListener
    {
        [SerializeField] private SpeakerID id;
        [SerializeField] private MoodStatValue[] moodImpact;

        public SpeakerID ID { get { return id; } }
        public MoodStatValue[] MoodImpact { get { return moodImpact; } }
    }

    [Serializable]
    public struct SpeakerSprite
    {
        [SerializeField] private Mood.Mood moodVariant;
        [SerializeField] private Sprite image;

        public Mood.Mood MoodVariant { get { return moodVariant; } }
        public Sprite Image { get { return image; } }
    }

    [Serializable]
    public struct Alias
    {
        [SerializeField] private string displayName;
        [SerializeField] private Color textColor;
        [SerializeField] private SpeakerSprite[] speakerSprites;

        public string DisplayName { get { return displayName; } }
        public Color TextColor { get {  return textColor; } }
        public SpeakerSprite[] SpeakerSprites { get {  return speakerSprites; } }
    }
}

namespace DiceyDialogue.Cards
{
    public enum CardID
    {
        None,
        SoulSearch,
        Breathe,
        Probe,
        Affirm,
        Challenge,
        Sucker,
        DoubleDown,
        Tattle
    }

    public enum CardState
    {
        Usable,
        Selected,
        Used,
        Unusable
    }

    [Serializable]
    public struct CardStateDisplay
    {

        [SerializeField] private float height, depth, scale, angleHeightScalar;
        [SerializeField] private Color tint;

        public float Height { get { return height; } }
        public float Depth { get { return depth; } }
        public float Scale { get { return scale; } }
        public float AngleHeightScalar { get { return angleHeightScalar; } }

        public Color Tint { get {  return tint; } }
    }
}