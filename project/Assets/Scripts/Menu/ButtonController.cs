using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum ButtonType
{
    START_GAME,
    QUIT_GAME,
    MAIN_MENU,
    RESTART,
    RESUME_GAME
}

[RequireComponent(typeof(Button))]
public class ButtonController : MonoBehaviour
{
    [SerializeField] private ButtonType buttonType;

    CanvasManager canvasManager;
    Button menuButton;

    private LevelLoader _levelLoader;

    private void Start()
    {
        menuButton = GetComponent<Button>();
        menuButton.onClick.AddListener(OnButtonClicked);
        canvasManager = CanvasManager.GetInstance();
        //_levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }

    void OnButtonClicked()
    {
        Scene scene = SceneManager.GetActiveScene();
        switch (buttonType)
        {
            case ButtonType.START_GAME:
                //Call other code that is necessary to start the game like levelManager.StartGame()
                //canvasManager.SwitchCanvas(CanvasType.Game);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                //_levelLoader.loadLevel("UI_Testing");
                //canvasManager.SwitchCanvas(CanvasType.EmptyCanvas);
                //_levelLoader.loadLevel((int)SceneIndexes.LEVEL_1);
                Time.timeScale = 1f;
                KeyController.GameIsPaused = false;
                break;
            case ButtonType.RESTART:
                SceneManager.LoadScene(scene.name);
                //_levelLoader.loadLevel(scene.name);
                Time.timeScale = 1f;
                KeyController.GameIsPaused = false;
                break;
            case ButtonType.RESUME_GAME:
                GameObject.Find("3rdPersonCamera").transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                canvasManager.SwitchCanvas(CanvasType.GameUI);
                Time.timeScale = 1f;
                KeyController.GameIsPaused = false;
                break;
            case ButtonType.MAIN_MENU:
                SceneManager.LoadScene("MainMenu");
                break;
            case ButtonType.QUIT_GAME:
                Debug.Log("Quitting...");
                Application.Quit();
                break;
            default:
                break;
        }
    }
}