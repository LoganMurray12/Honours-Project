using UnityEngine;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class Label : MonoBehaviour
{
    public string labelID;

    [Header("Audio")]
    public AudioClip correctClip;
    public AudioClip wrongClip;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private AudioSource audioSource;

    private bool isPlaced = false;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Label colliding with target and checking ID
        if (isPlaced) return;

        LabelSnapTarget target = other.GetComponent<LabelSnapTarget>();

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
        // Play correct sound
        if (audioSource != null && correctClip != null)
        {
            audioSource.PlayOneShot(correctClip);
        }
        // Handles snapping to the target object
        transform.position = target.transform.position;
        transform.rotation = target.transform.rotation;

        target.occupied = true;
        isPlaced = true;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        grabInteractable.enabled = false;

        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
    }

    private void WrongPlacement()
    {
        // Play wrong sound
        if (audioSource != null && wrongClip != null)
        {
            audioSource.PlayOneShot(wrongClip);
        }

        StartCoroutine(ResetAfterDelay(0.5f));
    }

    private System.Collections.IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetLabel();
    }

    public void ResetLabel()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}