using System.Collections.Generic;
using UnityEngine;

public class SpawnerDoorController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The door that will be opened.")]
    [SerializeField] private Door doorToOpen;

    [Tooltip("The list of spawners that must be defeated to open the door.")]
    [SerializeField] private List<EnemySpawner> spawnersToMonitor;

    private bool doorHasBeenOpened = false;

    private void Update()
    {
        // If the door is already open, do nothing.
        if (doorHasBeenOpened)
        {
            return;
        }

        // Check the list of spawners and remove any that have been destroyed.
        // We loop backwards because we are modifying the list while iterating.
        for (int i = spawnersToMonitor.Count - 1; i >= 0; i--)
        {
            if (spawnersToMonitor[i] == null)
            {
                spawnersToMonitor.RemoveAt(i);
            }
        }

        // If all spawners in the list have been defeated and removed
        if (spawnersToMonitor.Count == 0)
        {
            // Open the door and ensure this logic doesn't run again.
            doorToOpen.OpenDoor();
            doorHasBeenOpened = true;
        }
    }
}