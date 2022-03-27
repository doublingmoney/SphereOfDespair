using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField]
    private float _restartDelay = 3f;
    [SerializeField]
    private GameObject deathUI;
    [SerializeField]
    private GameObject tutorialUI;
    [SerializeField]
    private GameObject tutorialUIDash;
    [SerializeField]
    private GameObject tutorialUIBrake;

    private bool gameEnded = false;
    public void EndGame()
    {
        if (!gameEnded)
        {
            gameEnded = true;
            //Debug.Log("GAME OVER");
            Invoke("Restart", _restartDelay);
        }
    }

    public void FinishLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayerDied()
    {
        deathUI.SetActive(true);

    }

    public void ShowTutorial(int ID, bool show)
    {
        if (ID == 1)
        {
            tutorialUI.SetActive(show);
        }
        if (ID == 2)
        {
            tutorialUIDash.SetActive(show);
        }
        if (ID == 3)
        {
            tutorialUIBrake.SetActive(show);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        deathUI.SetActive(false);
    }

}
