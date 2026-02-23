using UnityEngine;

public class Display : MonoBehaviour
{
    public GameViewManager viewManager;
    protected RectTransform rectTransform;

    void Awake()
    {
        SetupRectTransform();
    }

    protected void SetupRectTransform()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public RectTransform GetRect()
    {
        return rectTransform;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
