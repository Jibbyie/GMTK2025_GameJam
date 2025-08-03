using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PressurePlate : MonoBehaviour
{
    [SerializeField] private Door door;
    [SerializeField] private AudioClip activationSfx;
    [SerializeField] private AudioClip deactivationSfx;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Weight"))
        {
            door.OpenDoor();
            audioSource.PlayOneShot(activationSfx);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Weight"))
        {
            door.CloseDoor();
            audioSource.PlayOneShot(deactivationSfx);
        }
    }
}