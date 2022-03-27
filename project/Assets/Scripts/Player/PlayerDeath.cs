using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    private void playerDied()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
