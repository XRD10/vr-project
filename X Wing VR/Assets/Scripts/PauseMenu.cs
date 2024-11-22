using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PauseMenu : MonoBehaviour
{
    public Canvas pauseMenu;
    public Button continueButton;
    public Button menuButton;

    public GameObject leftNearFarInteractor;
    public GameObject rightNearFarInteractor;

    private FlightControls flightControls;

    private void Awake()
    {
        flightControls = new FlightControls();
    }

    private void OnEnable()
    {
        // Subscribe to the event
        flightControls.Flying.ToggleMenu.Enable();
        flightControls.Flying.ToggleMenu.performed += ToggleMenu;
    }

    private void OnDisable()
    {
        // Unsubscribe from the event
        flightControls.Flying.ToggleMenu.performed -= ToggleMenu;
        flightControls.Flying.ToggleMenu.Disable();
    }

    void Start()
    {
        if (pauseMenu != null)
        {
            pauseMenu.enabled = false;
        }

        farCastEnable(false);

        if (continueButton != null)
        {
            continueButton.onClick.AddListener(Continue);
        }

        if (menuButton != null)
        {
            menuButton.onClick.AddListener(NavigateToMainMenu);
        }
    }

    public void ToggleMenu(InputAction.CallbackContext context)
    {
        if (pauseMenu != null)
        {
            pauseMenu.enabled = !pauseMenu.enabled;
            farCastEnable(pauseMenu.enabled);
        }
    }

    public void Continue()
    {
        farCastEnable(false);
        if (pauseMenu != null)
        {
            pauseMenu.enabled = false;
        }
    }

    public void NavigateToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void farCastEnable(bool enable)
    {
        if (leftNearFarInteractor != null)
        {
            leftNearFarInteractor.GetComponent<NearFarInteractor>().enableFarCasting = enable;
        }

        if (rightNearFarInteractor != null)
        {
            rightNearFarInteractor.GetComponent<NearFarInteractor>().enableFarCasting = enable;
        }
    }
}
