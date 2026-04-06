using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WheelHandleTurner : MonoBehaviour
{
    [Header("Grab Handle")]
    public XRGrabInteractable grabHandle;

    [Header("Wheel Settings")]
    public float sensitivity = 1f;
    public float minAngle = -180f;
    public float maxAngle = 180f;

    private bool isGrabbed = false;
    private Transform interactorTransform;
    private float previousHandAngle;
    private float currentWheelAngle;

    private void OnEnable()
    {
        if (grabHandle != null)
        {
            grabHandle.selectEntered.AddListener(OnGrabbed);
            grabHandle.selectExited.AddListener(OnReleased);
        }
    }

    private void OnDisable()
    {
        if (grabHandle != null)
        {
            grabHandle.selectEntered.RemoveListener(OnGrabbed);
            grabHandle.selectExited.RemoveListener(OnReleased);
        }
    }

    private void Update()
    {
        if (!isGrabbed || interactorTransform == null)
            return;

        Vector3 localHandPos = transform.InverseTransformPoint(interactorTransform.position);

        float currentHandAngle = Mathf.Atan2(localHandPos.z, localHandPos.x) * Mathf.Rad2Deg;
        float deltaAngle = Mathf.DeltaAngle(previousHandAngle, currentHandAngle);

        previousHandAngle = currentHandAngle;

        currentWheelAngle += deltaAngle * sensitivity;
        currentWheelAngle = Mathf.Clamp(currentWheelAngle, minAngle, maxAngle);

        transform.localRotation = Quaternion.Euler(0f, currentWheelAngle, 0f);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        interactorTransform = args.interactorObject.transform;

        Vector3 localHandPos = transform.InverseTransformPoint(interactorTransform.position);
        previousHandAngle = Mathf.Atan2(localHandPos.z, localHandPos.x) * Mathf.Rad2Deg;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        isGrabbed = false;
        interactorTransform = null;
    }
}