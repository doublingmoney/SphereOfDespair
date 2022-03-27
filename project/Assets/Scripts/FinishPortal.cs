using UnityEngine;

public class FinishPortal : MonoBehaviour
{
    private void Start()
    {
        GameEvents.Instance.onGoalsCompleted += LevelComplete;
    }

    private void LevelComplete()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
