using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game Object Pool of Displays.
/// </summary>
public class DisplayPool : MonoBehaviour
{
    /// <summary>
    /// The in-game View Manager.
    /// </summary>
    [SerializeField] private GameViewManager viewManager;

    /// <summary>
    /// The prefab to spawn.
    /// </summary>
    [SerializeField] private Display prefab;

    /// <summary>
    /// the parent to spawn the prefabs under.
    /// </summary>
    [SerializeField] private Transform spawnParent;

    /// <summary>
    /// The number of prefabs to spawn when the pool is awoken.
    /// </summary>
    [SerializeField] private int amountToSpawnAtAwake;

    /// <summary>
    /// List of the displays this pool has spawned.
    /// </summary>
    private List<Display> displays;

    /// <summary>
    /// Initiallizes displays list and spawns initial displays.
    /// </summary>
    public void Awake()
    {
        displays = new List<Display>();

        for (int i = 0; i < amountToSpawnAtAwake; i++)
        {
            SpawnNewDisplay();
        }
    }

    /// <summary>
    /// Get a currently inactive Display from the pool.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// "Depspawn" a display by making it inactive.
    /// </summary>
    /// <param name="display">The display to "Despawn"</param>
    public void RemoveDisplay(Display display)
    {
        display.gameObject.SetActive(false);
    }

    /// <summary>
    /// Instantiate a new Display for the pool to have.
    /// </summary>
    /// <returns>Reference to the Display</returns>
    public Display SpawnNewDisplay()
    {
        Display display = Instantiate(prefab, spawnParent);
        display.viewManager = viewManager;
        displays.Add(display);
        display.gameObject.SetActive(false);
        return display;
    }
}
