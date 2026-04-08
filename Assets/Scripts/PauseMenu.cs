using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Transform playerCamera;
    public float menuDistance = 2f;

    [Header("Input")]
    public InputActionReference pauseAction;

    private bool isPaused = false;

    void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.Enable();
            pauseAction.action.performed += OnPausePressed;
        }
    }

    void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.action.performed -= OnPausePressed;
            pauseAction.action.Disable();
        }
    }

    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        TogglePauseMenu();
    }

    public void TogglePauseMenu()
    {
        if (isPaused)
            HideMenu();
        else
            ShowMenu();
    }

    public void ShowMenu()
    {
        pauseMenuUI.SetActive(true);

        if (playerCamera != null)
        {
            Vector3 forward = playerCamera.forward;
            forward.y = 0f;
            forward.Normalize();

            pauseMenuUI.transform.position = playerCamera.position + forward * menuDistance;
            pauseMenuUI.transform.LookAt(playerCamera);
            pauseMenuUI.transform.Rotate(0f, 180f, 0f);
        }

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void HideMenu()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void RestartExperience()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}