using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    public Button continueButton;
    public Button menuButton;

    [SerializeField]
    private GameObject pauseMenu;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        pauseMenu.SetActive(true);
        continueButton.onClick.AddListener(HidePauseMenu);
        menuButton.onClick.AddListener(NavigateToMainMenu);
    }

    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    public void HidePauseMenu()
    {
        Debug.Log("Hide Pause Menu");

    }

    public void NavigateToMainMenu()
    {
        Debug.Log("Navigate to Main Menu");
        SceneManager.LoadScene(1);
    }

}
