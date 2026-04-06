using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WheelHandleTurner : MonoBehaviour
{
    public XRGrabInteractable grabHandle;
    public float sensitivity = 1f;
    public float minAngle = -90f;
    public float maxAngle = 90f;

    private bool isGrabbed = false;
    private Transform interactorTransform;
    private float previousHandAngle;
    private float currentWheelAngle;

    void OnEnable()
    {
        if (grabHandle != null)
        {
            grabHandle.selectEntered.AddListener(OnGrabbed);
            grabHandle.selectExited.AddListener(OnReleased);
        }
    }

    void OnDisable()
    {
        if (grabHandle != null)
        {
            grabHandle.selectEntered.RemoveListener(OnGrabbed);
            grabHandle.selectExited.RemoveListener(OnReleased);
        }
    }

    void Update()
    {
        if (!isGrabbed || interactorTransform == null)
            return;

        Vector3 localHandPos = transform.InverseTransformPoint(interactorTransform.position);

        // For Y-axis wheel rotation
        float currentHandAngle = Mathf.Atan2(localHandPos.z, localHandPos.x) * Mathf.Rad2Deg;
        float deltaAngle = Mathf.DeltaAngle(previousHandAngle, currentHandAngle);

        previousHandAngle = currentHandAngle;

        currentWheelAngle += deltaAngle * sensitivity;
        currentWheelAngle = Mathf.Clamp(currentWheelAngle, minAngle, maxAngle);

        transform.localRotation = Quaternion.Euler(0f, currentWheelAngle, 0f);
    }

    void OnGrabbed(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        interactorTransform = args.interactorObject.transform;

        Vector3 localHandPos = transform.InverseTransformPoint(interactorTransform.position);
        previousHandAngle = Mathf.Atan2(localHandPos.z, localHandPos.x) * Mathf.Rad2Deg;
    }

    void OnReleased(SelectExitEventArgs args)
    {
        isGrabbed = false;
        interactorTransform = null;
    }
}