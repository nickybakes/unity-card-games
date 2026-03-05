using UnityEngine;

/// <summary>
/// An element that rotated back and forth based on a given profile.
/// </summary>
public class RotatingElement : MonoBehaviour
{
    /// <summary>
    /// The profile to follow.
    /// </summary>
    [SerializeField] private RotatingElementProfile profile;

    /// <summary>
    /// How much this effect is used.
    /// </summary>
    [SerializeField, Range(0.0f, 1.0f)] private float effectAmount = 1;

    /// <summary>
    /// Updates rotation.
    /// </summary>
    void Update()
    {
        float lerpT = Mathf.Sin(Time.time * profile.TimeScale);
        lerpT *= lerpT;
        transform.localRotation = Quaternion.Euler(Vector3.Lerp(profile.RotationStart, profile.RotationEnd, lerpT) * effectAmount);
    }
}
