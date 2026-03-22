using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public SlidingDoor slidingDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            slidingDoor.PlayerEntered();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            slidingDoor.PlayerExited();
        }
    }
}
