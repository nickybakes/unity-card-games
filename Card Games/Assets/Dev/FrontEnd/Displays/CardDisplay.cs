using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : TravelingDisplay
{
    [SerializeField] private Animator animator;

    [SerializeField] private List<TextMeshProUGUI> valueDisplays;
    [SerializeField] private List<Image> suitDisplays;
    [SerializeField] private Image jackCorner;
    [SerializeField] private Image queenCorner;
    [SerializeField] private Image kingCorner;
    [SerializeField] private Image faceCustomImage;

    [SerializeField, Range(0.0f, 1.0f)] private float sizeTransition;

    [SerializeField] private float timeBeforeUpdateCardStateMin;
    [SerializeField] private float timeBeforeUpdateCardStateMax;

    public float SizeTransition { get => sizeTransition; }

    private Card card;

    private bool shownFlipped;
    private bool shownHeld;

    private bool updatedAfterInitialDisplay;

    private float timeDisplayed;

    private float timeBeforeUpdateCardState;

    public void DisplayCard(Card _card, CardVisualProfile visualProfile, RectTransform startingTransform, bool startFlipped)
    {
        card = _card;
        ApplyCardVisualProfile(visualProfile);

        timeDisplayed = 0;
        timeBeforeUpdateCardState = UnityEngine.Random.Range(timeBeforeUpdateCardStateMin, timeBeforeUpdateCardStateMax);

        SetStartTransform(startingTransform);
        ApplyStartTransform();

        gameObject.SetActive(true);

        updatedAfterInitialDisplay = false;

        UpdateHeld();

        shownFlipped = startFlipped;
        animator.SetBool("Flipped", shownFlipped);
        animator.SetTrigger("FlipInstant");
    }

    public void ApplyCardVisualProfile(CardVisualProfile visualProfile)
    {
        Sprite[] suitSprites = visualProfile.SuitSprites;
        Color[] suitSpriteColors = visualProfile.SuitSpriteColors;
        TMP_ColorGradient[] suitTextColors = visualProfile.SuitTextColors;

        foreach (TextMeshProUGUI text in valueDisplays)
        {
            text.text = Card.CARD_VALUE_STRINGS[(int)card.Value];
            text.colorGradientPreset = suitTextColors[(int)card.Suit];
        }

        foreach (Image image in suitDisplays)
        {
            image.sprite = suitSprites[(int)card.Suit];
            image.color = suitSpriteColors[(int)card.Suit];
        }

        jackCorner.gameObject.SetActive(false);
        queenCorner.gameObject.SetActive(false);
        kingCorner.gameObject.SetActive(false);
        faceCustomImage.gameObject.SetActive(false);

        switch (card.Value)
        {
            case CardValue.Jack:
                if (!visualProfile.DisableFaceCornerStyle)
                    jackCorner.gameObject.SetActive(true);
                if (visualProfile.JackCustomSprite != null)
                {
                    faceCustomImage.gameObject.SetActive(true);
                    faceCustomImage.sprite = visualProfile.JackCustomSprite;
                    if (visualProfile.TintCustomFaceSpriteWithSuitColor)
                    {
                        faceCustomImage.color = suitSpriteColors[(int)card.Suit];
                    }
                }
                break;

            case CardValue.Queen:
                if (!visualProfile.DisableFaceCornerStyle)
                    queenCorner.gameObject.SetActive(true);
                if (visualProfile.QueenCustomSprite != null)
                {
                    faceCustomImage.gameObject.SetActive(true);
                    faceCustomImage.sprite = visualProfile.QueenCustomSprite;
                    if (visualProfile.TintCustomFaceSpriteWithSuitColor)
                    {
                        faceCustomImage.color = suitSpriteColors[(int)card.Suit];
                    }
                }
                break;

            case CardValue.King:
                if (!visualProfile.DisableFaceCornerStyle)
                    kingCorner.gameObject.SetActive(true);
                if (visualProfile.KingCustomSprite != null)
                {
                    faceCustomImage.gameObject.SetActive(true);
                    faceCustomImage.sprite = visualProfile.KingCustomSprite;
                    if (visualProfile.TintCustomFaceSpriteWithSuitColor)
                    {
                        faceCustomImage.color = suitSpriteColors[(int)card.Suit];
                    }
                }
                break;
        }
    }

    public void SelectCard()
    {
        viewManager.CardSelected(card);
    }

    public void UpdateHeld(bool forceUpdate = false)
    {
        if (shownHeld != card.Held || forceUpdate)
        {
            shownHeld = card.Held;
            animator.SetBool("Held", shownHeld);
            animator.SetTrigger("ChangeHold");
        }
    }

    public void UpdateFlip(bool forceUpdate = false)
    {
        if (shownFlipped != card.Flipped || forceUpdate)
        {
            shownFlipped = card.Flipped;
            animator.SetBool("Flipped", shownFlipped);
            animator.SetTrigger("Flip");
        }
    }

    public void FlipOverride(bool overrideFlip)
    {
        shownFlipped = overrideFlip;
        animator.SetBool("Flipped", shownFlipped);
        animator.SetTrigger("Flip");
    }

    void Awake()
    {
        SetupRectTransform();
        SetupTravelingTransformData();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTravel();

        timeDisplayed += Time.deltaTime;

        if (timeDisplayed >= timeBeforeUpdateCardState && !updatedAfterInitialDisplay)
        {
            updatedAfterInitialDisplay = true;
            UpdateHeld();
            UpdateFlip();
        }
    }
}
