using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


public class PauseMenu : MonoBehaviour
{
    public Canvas pauseMenu;
    public InputActionAsset inputActions;
    public Button continueButton;
    public Button menuButton;

    public GameObject leftNearFarInteractor;
    public GameObject rightNearFarInteractor;


    private InputAction toggleMenuButton;

    private FlightControls flightControls;


    private void Awake()
    {
        flightControls = new FlightControls();
    }


    void Start()
    {
        pauseMenu.enabled = false;
        farCastEnable(false);
        continueButton.onClick.AddListener(Continue);
        menuButton.onClick.AddListener(NavigateToMainMenu);
        flightControls.Flying.ToggleMenu.Enable();
        flightControls.Flying.ToggleMenu.performed += ToggleMenu;
    }

    public void ToggleMenu(InputAction.CallbackContext context)
    {
        pauseMenu.enabled = !pauseMenu.enabled;
        farCastEnable(pauseMenu.enabled);
    }

    public void Continue()
    {
        farCastEnable(false);
        pauseMenu.enabled = false;
    }

    public void NavigateToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void farCastEnable(bool enable)
    {
        leftNearFarInteractor.GetComponent<NearFarInteractor>().enableFarCasting = enable;
        rightNearFarInteractor.GetComponent<NearFarInteractor>().enableFarCasting = enable;
    }

}
