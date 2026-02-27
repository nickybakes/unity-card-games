using System.Collections.Generic;
using UnityEngine;

public class PaytablePanel : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TravelingDisplay travelingDisplay;
    [SerializeField] private PaytableTextGroup namesTextGroup;
    [SerializeField] private PaytableTextGroup amountTextGroupPrefab;
    [SerializeField] private int numberOfBufferTextGroupsInDisplay = 2;
    [SerializeField] private string bufferString = "..................";

    private List<PaytableTextGroup> amountTextGroups;

    private float amountTextGroupWidth;

    private int currentlyHighlightedTextIndex = -1;

    private int chosenBetIndex;

    public void SetupPaytablePanel(PaytableDataBase paytableData)
    {
        if (UserManager.user == null)
            return;

        amountTextGroupWidth = amountTextGroupPrefab.GetComponent<RectTransform>().rect.width;
        amountTextGroups = new List<PaytableTextGroup>();

        for (int i = 0; i < paytableData.GetRowCount(); i++)
        {
            namesTextGroup.AddTextDisplay(paytableData.GetRowName(i));
        }

        for (int i = -numberOfBufferTextGroupsInDisplay; i < UserManager.user.NumberPossibleBets; i++)
        {
            PaytableTextGroup newAmountTextGroup = Instantiate(amountTextGroupPrefab, travelingDisplay.transform).GetComponent<PaytableTextGroup>();
            amountTextGroups.Add(newAmountTextGroup);

            for (int j = 0; j < paytableData.GetRowCount(); j++)
            {
                if (i < 0)
                {
                    newAmountTextGroup.AddTextDisplay(bufferString);
                }
                else
                {
                    float amount = UserManager.user.GetPossibleBet(i) * paytableData.GetBetMultiplier(j);

                    // Only show decimal values if they are needed. Makes the pay table look a little cleaner
                    if (amount != (int)amount)
                        newAmountTextGroup.AddTextDisplay(amount.ToString("C", UserManager.user.Culture));
                    else
                        newAmountTextGroup.AddTextDisplay(amount.ToString("C0", UserManager.user.Culture));
                }
            }
        }
    }

    public void UpdatePaytableChosenBet()
    {
        if (UserManager.user == null)
            return;

        chosenBetIndex = UserManager.user.CurrentSelectedBetIndex;
        travelingDisplay.TravelToTransform(new Vector2((chosenBetIndex + numberOfBufferTextGroupsInDisplay) * -amountTextGroupWidth, 0), Quaternion.identity, Vector3.one);
    }

    public void HighlightText(int index)
    {
        currentlyHighlightedTextIndex = index;
        namesTextGroup.HighlightText(index);
        amountTextGroups[chosenBetIndex + numberOfBufferTextGroupsInDisplay].HighlightText(index);
    }

    public void UnhighlightText()
    {
        if (currentlyHighlightedTextIndex != -1)
        {
            namesTextGroup.UnhighlightText(currentlyHighlightedTextIndex);
            amountTextGroups[chosenBetIndex + numberOfBufferTextGroupsInDisplay].UnhighlightText(currentlyHighlightedTextIndex);
            currentlyHighlightedTextIndex = -1;
        }
    }

    public void MaximizePanel()
    {
        UpdatePaytableChosenBet();
        animator.SetTrigger("Maximize");
    }

    public void MinimizePanel()
    {
        animator.SetTrigger("Minimize");
    }

    // Update is called once per frame
    void Update()
    {
        travelingDisplay.UpdateTravel();
    }
}
