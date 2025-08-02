using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip activationSfx;

    private AudioSource audioSource;
    private bool hasBeenTriggered = false; 

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered is the player AND if the sound hasn't played yet
        if (other.CompareTag("Player") && !hasBeenTriggered)
        {
            // Mark as triggered so it won't play again
            hasBeenTriggered = true;

            // Play the sound effect
            if (activationSfx != null)
            {
                audioSource.PlayOneShot(activationSfx);
            }
            Debug.Log("Respawn point set!");
        }
    }
}