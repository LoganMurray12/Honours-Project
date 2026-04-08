using UnityEngine;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
[RequireComponent(typeof(Rigidbody))]
public class Label : MonoBehaviour
{
    public string labelID;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Rigidbody rb;

    private bool isPlaced = false;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPlaced) return;

        LabelSnapTarget target = other.GetComponent<LabelSnapTarget>();

        if (target != null &&
            !target.occupied &&
            target.correctLabelID == labelID)
        {
            SnapToTarget(target);
        }
    }

    private void SnapToTarget(LabelSnapTarget target)
    {
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

    public void ResetLabel()
    {
        if (isPlaced) return;

        transform.position = startPosition;
        transform.rotation = startRotation;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}