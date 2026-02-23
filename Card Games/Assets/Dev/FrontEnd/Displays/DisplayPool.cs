using System.Collections.Generic;
using UnityEngine;

public class DisplayPool : MonoBehaviour
{
    [SerializeField] private GameViewManager viewManager;
    [SerializeField] private Display prefab;
    [SerializeField] private Transform spawnParent;
    [SerializeField] private int amountToSpawnAtAwake;

    private List<Display> displays;

    public void Awake()
    {
        displays = new List<Display>();

        for (int i = 0; i < amountToSpawnAtAwake; i++)
        {
            SpawnNewDisplay();
        }
    }

    public Display GetDisplay()
    {
        foreach (Display display in displays)
        {
            if (!display.gameObject.activeSelf)
            {
                return display;
            }
        }

        return SpawnNewDisplay();
    }

    public Display SpawnNewDisplay()
    {
        Display display = Instantiate(prefab, spawnParent);
        display.viewManager = viewManager;
        displays.Add(display);
        display.gameObject.SetActive(false);
        return display;
    }
}
