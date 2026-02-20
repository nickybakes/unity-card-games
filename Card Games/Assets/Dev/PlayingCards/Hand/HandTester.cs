using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HandTester : MonoBehaviour
{

    [SerializeField] private List<PokerHand> pokerHands;

    [SerializeField] private List<Card> hand01;
    [SerializeField] private List<Card> hand02;
    [SerializeField] private List<Card> hand03;
    [SerializeField] private List<Card> hand04;
    [SerializeField] private List<Card> hand05;
    [SerializeField] private List<Card> hand06;
    [SerializeField] private List<Card> hand07;
    [SerializeField] private List<Card> hand08;
    [SerializeField] private List<Card> hand09;
    [SerializeField] private List<Card> hand10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TestHands();
    }

    public void TestHands()
    {
        List<List<Card>> handsToTest = new List<List<Card>>()
        {
            hand01,
            hand02,
            hand03,
            hand04,
            hand05,
            hand06,
            hand07,
            hand08,
            hand09,
            hand10,
        };

        Debug.Log("----------");
        Debug.Log("Hand Tester Starting: " + gameObject.name);

        string pokerHandsToTestForString = "Poker Hands to test for: ";

        for (int i = 0; i < pokerHands.Count; i++)
        {
            pokerHandsToTestForString += pokerHands[i].Name;
            if (i < pokerHands.Count - 1)
                pokerHandsToTestForString += ", ";
        }

        Debug.Log(pokerHandsToTestForString);


        for (int i = 0; i < handsToTest.Count; i++)
        {
            if (handsToTest[i].Count == 0)
                continue;

            bool[] passed = new bool[pokerHands.Count];

            string resultString = "Hand " + (i + 1).ToString("D2") + ": ";

            for (int j = 0; j < pokerHands.Count; j++)
            {
                if (HandAnalysis.AnalizeForPokerHand(handsToTest[i], pokerHands[j]))
                {
                    passed[j] = true;
                    resultString += pokerHands[j].Name;
                    if (j < pokerHands.Count - 1)
                        resultString += ", ";
                }
            }

            Debug.Log(resultString);
        }

        Debug.Log("Done!");

#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // EditorApplication.isPlaying need to be set to false to end the game
        EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
