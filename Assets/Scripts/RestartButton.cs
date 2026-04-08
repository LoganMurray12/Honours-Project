using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class RestartOnButton : MonoBehaviour
{
    private InputAction restartAction;

    void Awake()
    {
        // B button on right controller
        restartAction = new InputAction(
            type: InputActionType.Button,
            binding: "<XRController>{RightHand}/secondaryButton"
        );
    }

    void OnEnable()
    {
        restartAction.Enable();
        restartAction.performed += OnRestartPressed;
    }

    void OnDisable()
    {
        restartAction.performed -= OnRestartPressed;
        restartAction.Disable();
    }

    private void OnRestartPressed(InputAction.CallbackContext context)
    {
        RestartLevel();
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}