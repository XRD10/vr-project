using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class PauseMenu : MonoBehaviour
{
    private Canvas pauseMenu;
    public InputActionAsset inputActions;
    public Button continueButton;
    public Button menuButton;
    private InputAction toggleMenuButton;

    private FlightControls flightControls;

    private void Awake()
    {
        flightControls = new FlightControls();
    }


    void Start()
    {
        pauseMenu = gameObject.GetComponent<Canvas>();
        pauseMenu.enabled = false;
        continueButton.onClick.AddListener(Continue);
        menuButton.onClick.AddListener(NavigateToMainMenu);
        // toggleMenuButton = inputActions.FindActionMap("Flying").FindAction("ToggleMenu");
        flightControls.Flying.ToggleMenu.Enable();
        flightControls.Flying.ToggleMenu.performed += ToggleMenu;
    }

    public void ToggleMenu(InputAction.CallbackContext context)
    {
        pauseMenu.enabled = !pauseMenu.enabled;
    }

    public void Continue()
    {
        Debug.Log("Hide Pause Menu");
        gameObject.SetActive(false);
    }

    public void NavigateToMainMenu()
    {
        Debug.Log("Navigate to Main Menu");
        SceneManager.LoadScene(0);
    }

}
