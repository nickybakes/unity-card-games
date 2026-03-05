using UnityEngine;

/// <summary>
/// Scriptable Object defining the idling rotation behavior of a Rotating Element.
/// </summary>
[CreateAssetMenu(fileName = "RotatingElementProfile", menuName = "Scriptable Objects/Rotating Element Profile")]
public class RotatingElementProfile : ScriptableObject
{
    /// <summary>
    /// The starting bound rotation.
    /// </summary>
    [field: SerializeField] public Vector3 RotationStart { get; private set; }

    /// <summary>
    /// The ending bound rotation.
    /// </summary>
    [field: SerializeField] public Vector3 RotationEnd { get; private set; }

    /// <summary>
    /// The time scale for lerping between the rotation bounds.
    /// </summary>
    [field: SerializeField] public float TimeScale { get; private set; } = 1;

}
