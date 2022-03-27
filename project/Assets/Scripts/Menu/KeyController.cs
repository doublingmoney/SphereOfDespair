using UnityEngine;

public class KeyController : MonoBehaviour
{
    CanvasManager canvasManager;
    public static bool GameIsPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        canvasManager = CanvasManager.GetInstance();
        canvasManager.SwitchCanvas(CanvasType.GameUI);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                resume();

            }
            else
            {
                pause();
            }
        }
    }

    public void resume()
    {
        GameObject.Find("3rdPersonCamera").transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        canvasManager.SwitchCanvas(CanvasType.GameUI);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void pause()
    {
        GameObject.Find("3rdPersonCamera").transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        canvasManager.SwitchCanvas(CanvasType.PauseMenu);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
