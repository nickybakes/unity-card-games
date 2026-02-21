using TMPro;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI valueDisplay;
    [SerializeField] private TextMeshProUGUI suitDisplay;

    public void DisplayCard(Card card)
    {
        valueDisplay.text = Card.CARD_VALUE_STRINGS[(int)card.Value];
        suitDisplay.text = Card.CARD_SUIT_STRINGS[(int)card.Suit];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
