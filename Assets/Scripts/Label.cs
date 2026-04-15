using UnityEngine;

// required components
[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class Label : MonoBehaviour
{
    // ID for each label for matching with target
    public string labelID;

    // sounds selection
    [Header("Audio")]
    public AudioClip correctClip;
    public AudioClip wrongClip;

    // ref for manager object
    [Header("Task Manager")]
    public LabelTaskManager taskManager;

    // storing position of labels
    private Vector3 startPosition;
    private Quaternion startRotation;

    // reference cache, apparently faster than using GetComponent
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private AudioSource audioSource;

    // tracking if the label is already placed
    private bool isPlaced = false;

    void Start()
    {
        // saves start position
        startPosition = transform.position;
        startRotation = transform.rotation;

        // grabs references from cache above
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // runs when object collides with another collider
    private void OnTriggerEnter(Collider other)
    {
        // if already placed do nothing
        if (isPlaced) return;

        // checks if its a valid target
        LabelSnapTarget target = other.GetComponent<LabelSnapTarget>();

        // if its valid, check if correct snap to target and if not refer to wrong placement
        if (target != null)
        {
            if (!target.occupied && target.correctLabelID == labelID)
            {
                SnapToTarget(target);
            }
            else
            {
                WrongPlacement();
            }
        }
    }

    private void SnapToTarget(LabelSnapTarget target)
    {
        // play sound
        if (audioSource != null && correctClip != null)
        {
            audioSource.PlayOneShot(correctClip);
        }

        // move into target position
        transform.position = target.transform.position;
        transform.rotation = target.transform.rotation;

        // locks object
        target.occupied = true;
        isPlaced = true;

        // stops physics movement
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // stops all physics
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        // stops ability to grab
        grabInteractable.enabled = false;

        // disables colliders so cant be bumped
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        // increases placement count for win condition
        if (taskManager != null)
        {
            taskManager.LabelPlaced();
        }
    }

    // plays wrong label sound
    private void WrongPlacement()
    {
        if (audioSource != null && wrongClip != null)
        {
            audioSource.PlayOneShot(wrongClip);
        }

        StartCoroutine(ResetAfterDelay(0.5f));
    }

    // resets object after short delay
    private System.Collections.IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetLabel();
    }

    // moves back to original position and stops movement
    public void ResetLabel()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}