using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private Door door;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Weight"))
        {
            door.OpenDoor();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Weight"))
        {
            door.CloseDoor();
        }
    }

}
