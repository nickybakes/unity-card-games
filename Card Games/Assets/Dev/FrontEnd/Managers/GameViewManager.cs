using System.Collections.Generic;
using UnityEngine;

public class GameViewManager : MonoBehaviour
{

    [SerializeField] private List<HandDisplay> handDisplays;
    [SerializeField] private List<DeckDisplay> deckDisplays;
    [SerializeField] private Transform gamePanelTransform;
    [SerializeField] private CardDisplay cardPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        List<Card> cards = new List<Card>(5)
        {
            new Card(CardValue.King, CardSuit.Heart, CardSpecial.None),
            new Card(CardValue.Five, CardSuit.Spade, CardSpecial.None),
            new Card(CardValue.Two, CardSuit.Heart, CardSpecial.None),
            new Card(CardValue.Five, CardSuit.Diamond, CardSpecial.None),
            new Card(CardValue.Jack, CardSuit.Club, CardSpecial.None)
        };

        foreach (Card card in cards)
        {
            SpawnCardPrefab(card);
        }
    }

    public void SpawnCardPrefab(Card card)
    {
        CardDisplay display = Instantiate(cardPrefab, gamePanelTransform);
        display.DisplayCard(card);
        handDisplays[0].AddCardDisplay(display);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
