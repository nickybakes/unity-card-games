using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// An in-game selectable element that is a bit more streamlined than the default Unity selectables.
/// </summary>
[RequireComponent(typeof(Animator))]
public class BetterSelectable :
        UIBehaviour,
        IPointerEnterHandler, IPointerExitHandler,
        ISelectHandler, IDeselectHandler
{
    private bool m_EnableCalled = false;

    [Tooltip("Can the Selectable be interacted with?")]
    [SerializeField]
    private bool m_Interactable = true;

    /// <summary>
    /// The event invoke when this is selected.
    /// </summary>
    [SerializeField] protected UnityEvent<int> selectEvent;

    /// <summary>
    /// The event invoke when this is deselected.
    /// </summary>
    [SerializeField] protected UnityEvent<int> deselectEvent;


    /// <summary>
    /// Useable index value thats passed on the select and deselect event.
    /// </summary>
    protected int index;

    /// <summary>
    /// The animator component on the selectable.
    /// </summary>
    protected Animator animator;

    /// <summary>
    /// Is this selectable interactable.
    /// </summary>
    public bool Interactable
    {
        get { return m_Interactable; }
        set
        {
            m_Interactable = value;
            if (!m_Interactable)
            {
                SetAnimationTrigger("Disable");
                Deselect();
            }
            else if (m_Interactable)
            {
                SetAnimationTrigger("Enable");
                if (isPointerInside)
                    Select();
            }
        }
    }

    /// <summary>
    /// Whether the pointer is inside the selectable or not.
    /// </summary>
    protected bool isPointerInside { get; set; }

    /// <summary>
    /// Whether this selectable is selected currently.
    /// </summary>
    protected bool hasSelection { get; set; }

    protected override void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Sets a trigger in the animator.
    /// </summary>
    /// <param name="trigger">The trigger to set.</param>
    public void SetAnimationTrigger(string trigger)
    {
        if (animator.enabled && animator.runtimeAnimatorController != null)
        {
            animator.SetTrigger(trigger);
        }
    }

    /// <summary>
    /// Resets a trigger in the animator.
    /// </summary>
    /// <param name="trigger">The trigger to reset.</param>
    public void ResetAnimationTrigger(string trigger)
    {
        if (animator.enabled && animator.runtimeAnimatorController != null)
        {
            animator.ResetTrigger(trigger);
        }
    }

    /// <summary>
    /// When the selectable is enabled.
    /// </summary>
    protected override void OnEnable()
    {
        //Check to avoid multiple OnEnable() calls for each selectable
        if (m_EnableCalled)
            return;

        base.OnEnable();

        if (EventSystem.current && EventSystem.current.currentSelectedGameObject == gameObject)
        {
            hasSelection = true;
        }

        m_EnableCalled = true;
    }

    /// <summary>
    /// When the selectable is disabled.
    /// </summary>
    protected override void OnDisable()
    {
        //Check to avoid multiple OnDisable() calls for each selectable
        if (!m_EnableCalled)
            return;

        InstantClearState();
        base.OnDisable();

        m_EnableCalled = false;
    }

    /// <summary>
    /// Clear any internal state from the Selectable (used when disabling).
    /// </summary>
    protected virtual void InstantClearState()
    {
        isPointerInside = false;
        hasSelection = false;
        OnReset();
    }

    /// <summary>
    /// Go back to the default animation.
    /// </summary>
    public virtual void OnReset()
    {
        SetAnimationTrigger("Reset");
    }

    /// <summary>
    /// If this is interactible and not selected, play the select animation.
    /// </summary>
    public virtual void OnSelected()
    {
        if (!hasSelection && Interactable)
        {
            SetAnimationTrigger("Select");
            selectEvent.Invoke(index);
            hasSelection = true;
        }
    }

    /// <summary>
    /// If this is selected, play the deselect animation.
    /// </summary>
    public virtual void OnDeselected()
    {
        if (hasSelection)
        {
            SetAnimationTrigger("Deselect");
            deselectEvent.Invoke(index);
            hasSelection = false;
        }
    }

    /// <summary>
    /// Evaluate current state and transition to appropriate state.
    /// New state could be pressed or hover depending on pressed state.
    /// </summary>
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        isPointerInside = true;
        OnSelected();
    }

    /// <summary>
    /// Evaluate current state and transition to normal state.
    /// </summary>
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        isPointerInside = false;
        OnDeselected();
    }

    /// <summary>
    /// Set selection and transition to appropriate state.
    /// </summary>
    public virtual void OnSelect(BaseEventData eventData)
    {
        if (Interactable)
            OnSelected();
    }

    /// <summary>
    /// Unset selection and transition to appropriate state.
    /// </summary>
    public virtual void OnDeselect(BaseEventData eventData)
    {
        OnDeselected();
    }

    /// <summary>
    /// Selects this Selectable.
    /// </summary>
    public virtual void Select()
    {
        if (EventSystem.current == null || EventSystem.current.alreadySelecting)
            return;

        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    /// <summary>
    /// Selects this Selectable.
    /// </summary>
    public virtual void Deselect()
    {
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    /// <summary>
    /// Plays the hide animation and makes it not interactible.
    /// </summary> 
    public void Hide()
    {
        Interactable = false;
        animator.SetTrigger("Hide");
    }

    /// <summary>
    /// Plays the show animation and makes it interactible.
    /// </summary>
    public void Show()
    {
        Interactable = true;
        animator.SetTrigger("Show");
    }
}
