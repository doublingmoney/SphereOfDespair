using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingBar;
    [SerializeField] private Text progressText;

    public void loadLevel(int sceneIndex)
    {
        //gameObject.transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(LoadYourAsyncScene(sceneIndex));
    }

    IEnumerator LoadYourAsyncScene(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        //loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            //Debug.Log(progress);
            //loadingBar.value = progress;
            //progressText.text = progress * 100f + "%";
            yield return null;
        }

        //gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
}
