using UnityEngine;

public class AssemblyTaskManager : MonoBehaviour
{
    public int totalParts = 4;
    public GameObject winMessageObject;

    private int placedParts = 0;

    void Start()
    {
        if (winMessageObject != null)
        {
            winMessageObject.SetActive(false);
        }
    }

    public void PartPlaced()
    {
        placedParts++;

        if (placedParts >= totalParts)
        {
            if (winMessageObject != null)
            {
                winMessageObject.SetActive(true);
            }
        }
    }
}
