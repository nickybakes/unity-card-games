using UnityEngine;

/// <summary>
/// A basic in-game UI component that has reference to its Rect Transform
/// </summary>
public class Display : MonoBehaviour
{
    /// <summary>
    /// Reference to the game's View Manager.
    /// </summary>
    public GameViewManager viewManager;

    /// <summary>
    /// Reference to this object's Rect Transform.
    /// </summary>
    protected RectTransform rectTransform;

    /// <summary>
    /// Set up rect transform data.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
    }

    /// <summary>
    /// Sets up rect transform data.
    /// </summary>
    protected void SetupRectTransform()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Get the Rect Transform.
    /// </summary>
    /// <returns>The Rect Transform of this Display.</returns>
    public RectTransform GetRect()
    {
        return rectTransform;
    }
}
