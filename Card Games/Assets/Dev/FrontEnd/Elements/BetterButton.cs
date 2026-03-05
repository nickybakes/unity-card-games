using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// An in-game button that is a bit more streamlined than the default Unity buttons.
/// </summary>
public class BetterButton : BetterSelectable, IPointerDownHandler, IPointerUpHandler, ISubmitHandler
{
    /// <summary>
    /// The event to call when this button is submitted (clicked).
    /// </summary>
    [SerializeField] protected UnityEvent submitEvent;

    /// <summary>
    /// Cooldown between when this button can be submitted again.
    /// </summary>
    [SerializeField] private float cooldownTime = .1f;

    /// <summary>
    /// The time since the last submission.
    /// </summary>
    private float timeSinceSubmit;

    /// <summary>
    /// Whether the button is currently held down/pressed.
    /// </summary>
    private bool pressed;

    /// <summary>
    /// Play the press and release animation and invoke the submit event.
    /// </summary>
    public virtual void Submit()
    {
        if (!IsActive() || !Interactable || timeSinceSubmit < cooldownTime)
            return;

        timeSinceSubmit = 0;

        SetAnimationTrigger("Press");
        SetAnimationTrigger("Release");

        pressed = false;

        submitEvent.Invoke();
    }

    /// <summary>
    /// When the user pressed down on the button.
    /// </summary>
    public virtual void Press()
    {
        if (!IsActive() || !Interactable)
            return;

        ResetAnimationTrigger("Release");
        SetAnimationTrigger("Press");
        pressed = true;
    }

    /// <summary>
    /// When the user releases the button from being held down. Only submit if the cursor is inside the button.
    /// </summary>
    public virtual void Release()
    {
        if (!IsActive() || !Interactable)
            return;

        SetAnimationTrigger("Release");
        pressed = false;
        if (isPointerInside)
        {
            Submit();
        }
    }

    /// <summary>
    /// Add handling for click-holding then dragging off/on the button.
    /// </summary>
    public override void OnSelected()
    {
        if (!IsActive() || !Interactable)
            return;

        if (!hasSelection)
        {
            if (pressed)
            {
                SetAnimationTrigger("Press");
            }
            else
            {
                SetAnimationTrigger("Select");
                selectEvent.Invoke(index);
            }
            hasSelection = true;
        }
    }

    void Update()
    {
        timeSinceSubmit += Time.deltaTime;
    }

    /// <summary>
    /// Evaluate current state and transition to pressed state.
    /// </summary>
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        // Selection tracking
        if (Interactable && EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(gameObject, eventData);

        Press();
    }

    /// <summary>
    /// Evaluate eventData and transition to appropriate state.
    /// </summary>
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        Release();
    }

    /// <summary>
    /// Call all registered ISubmitHandler.
    /// </summary>
    public virtual void OnSubmit(BaseEventData eventData)
    {
        Submit();
    }
}
