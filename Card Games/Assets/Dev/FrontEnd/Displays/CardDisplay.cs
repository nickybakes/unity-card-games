using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines.Interpolators;
using UnityEngine.UIElements;

public class CardDisplay : TravelingDisplay
{

    [SerializeField] private TextMeshProUGUI valueDisplay;
    [SerializeField] private TextMeshProUGUI suitDisplay;

    [SerializeField, Range(0.0f, 1.0f)] private float sizeTransition;

    public float SizeTransition { get => sizeTransition; }


    public void DisplayCard(Card card)
    {
        valueDisplay.text = Card.CARD_VALUE_STRINGS[(int)card.Value];
        suitDisplay.text = Card.CARD_SUIT_STRINGS[(int)card.Suit];
        gameObject.SetActive(true);
    }

    void Awake()
    {
        SetupRectTransform();
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
