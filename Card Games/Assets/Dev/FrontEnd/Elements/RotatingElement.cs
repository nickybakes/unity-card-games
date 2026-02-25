using UnityEngine;

public class IdlingElement : MonoBehaviour
{

    [SerializeField] private RotatingElementProfile profile;

    [SerializeField, Range(0.0f, 1.0f)] private float effectAmount = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float lerpT = Mathf.Sin(Time.time * profile.TimeScale);
        lerpT *= lerpT;
        transform.localRotation = Quaternion.Euler(Vector3.Lerp(profile.RotationStart, profile.RotationEnd, lerpT) * effectAmount);
    }
}
