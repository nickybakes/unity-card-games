using UnityEngine;

public class Display : MonoBehaviour
{
    protected RectTransform rectTransform;

    void Awake()
    {
        SetupRectTransform();
    }

    protected void SetupRectTransform()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
