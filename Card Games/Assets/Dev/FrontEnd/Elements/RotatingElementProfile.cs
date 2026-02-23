using UnityEngine;

[CreateAssetMenu(fileName = "RotatingElementProfile", menuName = "Scriptable Objects/Rotating Element Profile")]
public class RotatingElementProfile : ScriptableObject
{
    [field: SerializeField] public Vector3 RotationStart { get; private set; }

    [field: SerializeField] public Vector3 RotationEnd { get; private set; }

    [field: SerializeField] public float TimeScale { get; private set; } = 1;

}
