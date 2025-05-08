using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DiceyDialogue.Cards;

public class Card : MonoBehaviour
{
    [SerializeField] private Transform cardBase;
    [SerializeField] private BoxCollider hitBox;
    [SerializeField] private SpriteRenderer imageSprite, tintSprite;
    [SerializeField] private TMP_Text nameLabel, blurbLabel;
    [SerializeField] private CardDisplayData displayData;

    private Vector3 originalPos;
    private Vector3 originalScale;
    private Vector3 originalHotBoxPos;

    private CardData data;
    private CardState state;

    public CardData Data {  get { return data; } }
    public CardState State { get { return state; } }

    public void SetData(CardData newData)
    {
        data = newData;

        imageSprite.sprite = data.Image;
        nameLabel.text = data.DisplayName;

        blurbLabel.text = "";
        for (int i = 0; i < data.Blurbs.Length; i++)
        {
            blurbLabel.text += data.Blurbs[i];

            if (i < data.Blurbs.Length - 1)
            {
                blurbLabel.text += "\n\n";
            }
        }
    }

    public void SetState(CardState newState)
    {
        if (state == newState)
            return;

        state = newState;
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        CardStateDisplay display;
        float height = 0;

        switch (state)
        {
            default:
            case CardState.Usable:
                display = displayData.UsableCardDisplay;
                height = display.Height;

                hitBox.enabled = true;
                break;

            case CardState.Selected:
                display = displayData.SelectedCardDisplay;
                height = display.Height + (Mathf.Abs(transform.rotation.z) * display.AngleHeightScalar);

                hitBox.enabled = true;
                break;

            case CardState.Used:
                display = displayData.UsedCardDisplay;
                height = display.Height;

                hitBox.enabled = false;
                break;

            case CardState.Unusable:
                display = displayData.UnusableCardDisplay;
                height = display.Height;

                hitBox.enabled = false;
                break;
        }

        cardBase.localPosition = originalPos + new Vector3(0, height, display.Depth);
        cardBase.localScale = originalScale * display.Scale;

        hitBox.center = originalHotBoxPos + new Vector3(0, height, 0);

        tintSprite.color = display.Tint;
    }

    public void Awake()
    {
        originalPos = cardBase.localPosition;
        originalScale = cardBase.localScale;

        originalHotBoxPos = hitBox.center;
    }

    public void Update()
    {
        DebugRefreshDisplay();
    }

    private void DebugRefreshDisplay()
    {
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            UpdateCardDisplay();
        }
    }
}
