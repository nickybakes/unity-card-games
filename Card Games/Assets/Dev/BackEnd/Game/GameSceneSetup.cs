using UnityEngine;

public class GameSceneSetup : MonoBehaviour
{

    [field: SerializeField] public GameManagerBase GameManagerReference { get; private set; }
    [field: SerializeField] public GameViewManager GameViewManagerReference { get; private set; }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
