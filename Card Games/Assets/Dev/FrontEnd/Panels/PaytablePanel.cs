using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles showing the different reard amounts for different bets on the paytable.
/// </summary>
public class PaytablePanel : MonoBehaviour
{
    /// <summary>
    /// The animator that controls the Paytable Panel.
    /// </summary>
    [SerializeField] private Animator animator;

    /// <summary>
    /// The shifting paytable object that moves to center the player's bet.
    /// </summary>
    [SerializeField] private TravelingDisplay travelingDisplay;

    /// <summary>
    /// The group for displaying all the paytable reward names.
    /// </summary>
    [SerializeField] private PaytableTextGroup namesTextGroup;

    /// <summary>
    /// The group prefab to spawn for displaying one column of paytable rewards.
    /// </summary>
    [SerializeField] private PaytableTextGroup amountTextGroupPrefab;

    /// <summary>
    /// The number of buffer columns to put on the left side of the paytable.
    /// </summary>
    [SerializeField] private int numberOfBufferTextGroupsInDisplay = 2;

    /// <summary>
    /// Customizable string to fill the buffer columns with.
    /// </summary>
    [SerializeField] private string bufferString = "..................";

    /// <summary>
    /// The spawned text groups for each columnn.
    /// </summary>
    private List<PaytableTextGroup> amountTextGroups;

    /// <summary>
    /// The calculated width of each column.
    /// </summary>
    private float amountTextGroupWidth;

    /// <summary>
    /// The index of the currently highlighted row.
    /// </summary>
    private int currentlyHighlightedTextIndex = -1;

    /// <summary>
    /// The index of the currently chosen bet.
    /// </summary>
    private int chosenBetIndex;

    /// <summary>
    /// Spawns all the rows and columns to create the paytable.
    /// </summary>
    /// <param name="paytableData">The paytable data to follow.</param>
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

    /// <summary>
    /// Travels the paytable traveling display to center on the user's selected bet.
    /// </summary>
    public void UpdatePaytableChosenBet()
    {
        if (UserManager.user == null)
            return;

        chosenBetIndex = UserManager.user.CurrentSelectedBetIndex;
        travelingDisplay.TravelToTransform(new Vector2((chosenBetIndex + numberOfBufferTextGroupsInDisplay) * -amountTextGroupWidth, 0), Quaternion.identity, Vector3.one);
    }

    /// <summary>
    /// Highlights a row of text.
    /// </summary>
    /// <param name="index">The row index.</param>
    public void HighlightText(int index)
    {
        currentlyHighlightedTextIndex = index;
        namesTextGroup.HighlightText(index);
        amountTextGroups[chosenBetIndex + numberOfBufferTextGroupsInDisplay].HighlightText(index);
    }

    /// <summary>
    /// Unhighlights the currently highlighted row of text.
    /// </summary>
    public void UnhighlightText()
    {
        if (currentlyHighlightedTextIndex != -1)
        {
            namesTextGroup.UnhighlightText(currentlyHighlightedTextIndex);
            amountTextGroups[chosenBetIndex + numberOfBufferTextGroupsInDisplay].UnhighlightText(currentlyHighlightedTextIndex);
            currentlyHighlightedTextIndex = -1;
        }
    }

    /// <summary>
    /// Play the Maximize animation and update the paytable.
    /// </summary>
    public void MaximizePanel()
    {
        UpdatePaytableChosenBet();
        animator.SetTrigger("Maximize");
    }

    /// <summary>
    /// Play the Minimize animation. 
    /// </summary>
    public void MinimizePanel()
    {
        animator.SetTrigger("Minimize");
    }

    /// <summary>
    /// Update traveling data.
    /// </summary>
    void Update()
    {
        travelingDisplay.UpdateTravel();
    }
}
