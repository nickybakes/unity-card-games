using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class BetterSelectable :
        UIBehaviour,
        IPointerEnterHandler, IPointerExitHandler,
        ISelectHandler, IDeselectHandler
{
    private bool m_EnableCalled = false;

    private bool m_GroupsAllowInteraction = true;

    [Tooltip("Can the Selectable be interacted with?")]
    [SerializeField]
    private bool m_Interactable = true;

    [SerializeField] protected UnityEvent<int> selectEvent;
    [SerializeField] protected UnityEvent<int> deselectEvent;

    protected int index;


    protected Animator animator;


    /// <summary>
    /// Is this object interactable.
    /// </summary>
    /// <example>
    public bool interactable
    {
        get { return m_Interactable; }
        set
        {
            m_Interactable = value;
            if (!m_Interactable && EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
                EventSystem.current.SetSelectedGameObject(null);
        }
    }

    protected bool isPointerInside { get; set; }
    protected bool hasSelection { get; set; }

    protected override void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimationTrigger(string trigger)
    {
        if (animator.enabled && animator.runtimeAnimatorController != null)
        {
            animator.SetTrigger(trigger);
        }
    }

    public void ResetAnimationTrigger(string trigger)
    {
        if (animator.enabled && animator.runtimeAnimatorController != null)
        {
            animator.ResetTrigger(trigger);
        }
    }

    /// <summary>
    /// Is the object interactable.
    /// </summary>
    public virtual bool IsInteractable()
    {
        return m_GroupsAllowInteraction && m_Interactable;
    }

    // Select on enable and add to the list.
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

    // Remove from the list.
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

    public virtual void OnReset()
    {
        SetAnimationTrigger("Reset");
    }

    public virtual void OnSelected()
    {
        if (!hasSelection)
        {
            SetAnimationTrigger("Select");
            selectEvent.Invoke(index);
            hasSelection = true;
        }
    }

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
}
