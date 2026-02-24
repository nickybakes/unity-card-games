using System;
using TMPro;
using UnityEngine;
using UnityEngine.Splines.Interpolators;
using UnityEngine.UIElements;

public class CardDisplay : TravelingDisplay
{
    [SerializeField] private Animator animator;

    [SerializeField] private TextMeshProUGUI valueDisplay;
    [SerializeField] private TextMeshProUGUI suitDisplay;

    [SerializeField, Range(0.0f, 1.0f)] private float sizeTransition;

    public float SizeTransition { get => sizeTransition; }

    private Card card;

    private bool shownFlipped;
    private bool shownHeld;

    private bool updatedAfterInitialDisplay;

    public void DisplayCard(Card _card, RectTransform startingTransform, bool startFlipped)
    {
        card = _card;
        valueDisplay.text = Card.CARD_VALUE_STRINGS[(int)card.Value];
        suitDisplay.text = Card.CARD_SUIT_STRINGS[(int)card.Suit];

        SetStartTransform(startingTransform);
        ApplyStartTransform();

        gameObject.SetActive(true);

        updatedAfterInitialDisplay = false;

        UpdateHeld();

        shownFlipped = startFlipped;
        animator.SetBool("Flipped", shownFlipped);
        animator.SetTrigger("FlipInstant");
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

        if (!updatedAfterInitialDisplay)
        {
            updatedAfterInitialDisplay = true;
            UpdateFlip();
            UpdateHeld();
        }
    }
}
