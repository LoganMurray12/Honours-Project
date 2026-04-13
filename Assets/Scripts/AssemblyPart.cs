using UnityEngine;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class AssemblyPart : MonoBehaviour
{
    public string partID;

    [Header("Audio")]
    public AudioClip correctClip;
    public AudioClip wrongClip;

    [Header("Assembly")]
    public AssemblyTaskManager taskManager;
    public GameObject partLabel;

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

        if (partLabel != null)
        {
            partLabel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPlaced) return;

        AssemblySnapTarget target = other.GetComponent<AssemblySnapTarget>();

        if (target != null)
        {
            if (!target.occupied && target.correctPartID == partID)
            {
                SnapToTarget(target);
            }
            else
            {
                WrongPlacement();
            }
        }
    }

    private void SnapToTarget(AssemblySnapTarget target)
    {
        if (audioSource != null && correctClip != null)
        {
            audioSource.PlayOneShot(correctClip);
        }

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

        if (partLabel != null)
        {
            partLabel.SetActive(true);
        }

        if (taskManager != null)
        {
            taskManager.PartPlaced();
        }
    }

    private void WrongPlacement()
    {
        if (audioSource != null && wrongClip != null)
        {
            audioSource.PlayOneShot(wrongClip);
        }

        StartCoroutine(ResetAfterDelay(0.5f));
    }

    private System.Collections.IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetPart();
    }

    public void ResetPart()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}