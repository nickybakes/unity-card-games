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

    private bool flipped;


    public void DisplayCard(Card card, bool startFlipped = false)
    {
        valueDisplay.text = Card.CARD_VALUE_STRINGS[(int)card.Value];
        suitDisplay.text = Card.CARD_SUIT_STRINGS[(int)card.Suit];
        gameObject.SetActive(true);
        flipped = startFlipped;
        animator.SetBool("Flipped", flipped);
        animator.SetTrigger("FlipInstant");
    }

    public void FlipCard()
    {
        flipped = !flipped;
        animator.SetBool("Flipped", flipped);
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
    }
}
