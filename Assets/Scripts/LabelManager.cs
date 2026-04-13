using UnityEngine;

public class LabelTaskManager : MonoBehaviour
{
    public int totalLabels = 4;
    
    public GameObject winMessageObject;
    

    private int placedLabels = 0;
    

    void Start()
    {
        if (winMessageObject != null)
        {
            winMessageObject.SetActive(false);
        }
    }

    public void LabelPlaced()
    {
        placedLabels++;

        if (placedLabels >= totalLabels)
        {
            ShowWinMessage();
        }
    }

    

    private void ShowWinMessage()
    {
        if (winMessageObject != null)
        {
            winMessageObject.SetActive(true);
        }
    }

   
}