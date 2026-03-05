using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The in-game display of a Card.
/// </summary>
public class CardDisplay : TravelingDisplay
{
    /// <summary>
    /// The animator attached to this Card Display.
    /// </summary>
    [SerializeField] private Animator animator;

    /// <summary>
    /// Text elements that show the card's value.
    /// </summary>
    [SerializeField] private List<TextMeshProUGUI> valueDisplays;

    /// <summary>
    /// Image elements that show the card's suit.
    /// </summary>
    [SerializeField] private List<Image> suitDisplays;

    /// <summary>
    /// Corner graphic for Jack.
    /// </summary>
    [SerializeField] private Image jackCorner;

    /// <summary>
    /// Corner image graphic for Queen.
    /// </summary>
    [SerializeField] private Image queenCorner;

    /// <summary>
    /// Corner image graphic for King.
    /// </summary>
    [SerializeField] private Image kingCorner;

    /// <summary>
    /// Image graphic for display face card art.
    /// </summary>
    [SerializeField] private Image faceCustomImage;

    /// <summary>
    /// Transition lerp between unselected and selected size. Use the animator to control this.
    /// </summary>
    [SerializeField, Range(0.0f, 1.0f)] private float sizeTransition;

    /// <summary>
    /// The minimum amount of time after a card is drawn before updating its flipped and held status.
    /// </summary>
    [SerializeField] private float timeBeforeUpdateCardStateMin;

    /// <summary>
    /// The maximum amount of time after a card is drawn before updating its flipped and held status.
    /// </summary>
    [SerializeField] private float timeBeforeUpdateCardStateMax;

    /// <summary>
    /// Transition lerp between unselected and selected size. Use the animator to control this.
    /// </summary>
    public float SizeTransition { get => sizeTransition; }

    /// <summary>
    /// Reference to the Card that this Display represents.
    /// </summary>
    private Card card;

    /// <summary>
    /// Whether this display is shown flipped.
    /// </summary>
    private bool shownFlipped;

    /// <summary>
    /// Whether this display is shown held.
    /// </summary>
    private bool shownHeld;

    /// <summary>
    /// Whether the card's flipped/held status has been updated after being drawn.
    /// </summary>
    private bool updatedAfterInitialDisplay;

    /// <summary>
    /// The amount of time that this card has been displayed for.
    /// </summary>
    private float timeDisplayed;

    /// <summary>
    /// The time when the card should update its statuses.
    /// </summary>
    private float timeBeforeUpdateCardState;


    /// <summary>
    /// Setup basic travel data.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
        SetupTravelingTransformData();
    }

    /// <summary>
    /// Sets up the visuals to display a Card.
    /// </summary>
    /// <param name="_card">The Card to display/</param>
    /// <param name="visualProfile">The visual settings used for displaying the Card.</param>
    /// <param name="startingTransform">The starting position, rotation, and scale.</param>
    /// <param name="startFlipped">Whether the card should be started flipped.</param>
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

    /// <summary>
    /// Updates the graphics on the display to show the current Card, driven by the data in the visual profile.
    /// </summary>
    /// <param name="visualProfile">The settings that drive how the card should look.</param>
    public void ApplyCardVisualProfile(CardVisualProfile visualProfile)
    {
        Sprite[] suitSprites = visualProfile.SuitSprites;
        Color[] suitSpriteColors = visualProfile.SuitSpriteColors;
        TMP_ColorGradient[] suitTextColors = visualProfile.SuitTextColors;

        // Show Card Value in text
        foreach (TextMeshProUGUI text in valueDisplays)
        {
            text.text = Card.CARD_VALUE_STRINGS[(int)card.Value];
            text.colorGradientPreset = suitTextColors[(int)card.Suit];
        }

        // Show Card Suit in the images
        foreach (Image image in suitDisplays)
        {
            image.sprite = suitSprites[(int)card.Suit];
            image.color = suitSpriteColors[(int)card.Suit];
        }

        jackCorner.gameObject.SetActive(false);
        queenCorner.gameObject.SetActive(false);
        kingCorner.gameObject.SetActive(false);
        faceCustomImage.gameObject.SetActive(false);

        // Set face card specific sprites.
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

    /// <summary>
    /// Function to call when the player submits (clicks) this card.
    /// </summary>
    public void SubmitCard()
    {
        viewManager.CardSubmitted(card);
    }

    /// <summary>
    /// Updates the card's visuals to show whether its held or not.
    /// </summary>
    /// <param name="forceUpdate">Force the animation to play even if it doesn't need to update.</param>
    public void UpdateHeld(bool forceUpdate = false)
    {
        if (shownHeld != card.Held || forceUpdate)
        {
            shownHeld = card.Held;
            animator.SetBool("Held", shownHeld);
            animator.SetTrigger("ChangeHold");
        }
    }


    /// <summary>
    /// Updates the card's visuals to show whether its flipped or not.
    /// </summary>
    /// <param name="forceUpdate">Force the animation to play even if it doesn't need to update.</param>
    public void UpdateFlip(bool forceUpdate = false)
    {
        if (shownFlipped != card.Flipped || forceUpdate)
        {
            shownFlipped = card.Flipped;
            animator.SetBool("Flipped", shownFlipped);
            animator.SetTrigger("Flip");
        }
    }

    /// <summary>
    /// Flip the card even if the Card its displaying isnt actually flipped.
    /// </summary>
    /// <param name="overrideFlip">Whether the card should show flipped or not.</param>
    public void FlipOverride(bool overrideFlip)
    {
        shownFlipped = overrideFlip;
        animator.SetBool("Flipped", shownFlipped);
        animator.SetTrigger("Flip");
    }

    /// <summary>
    /// Update the traveling data of the card display.
    /// </summary>
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
