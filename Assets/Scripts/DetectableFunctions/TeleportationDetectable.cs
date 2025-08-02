using UnityEngine;

public class TeleportationDetectable : DetectableObject
{
    private Transform player;

    [Header("Audio")]
    [SerializeField] private AudioClip teleportSfx;
    private AudioSource audioSource;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();
    }

    public override void OnDetected()
    {
        if (teleportSfx != null && audioSource != null)
        {
            audioSource.PlayOneShot(teleportSfx);
        }

        player.transform.position = this.transform.position;
    }
}
